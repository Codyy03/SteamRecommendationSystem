import pandas as pd
import numpy as np
from sklearn.neighbors import NearestNeighbors
from sklearn.feature_extraction.text import TfidfVectorizer
import pickle

input_file = "data/steam_games_final.csv"

ccols_to_keep = [
    'appid', 'name', 'short_description', 'recommendations_total', 'genre_name',
    'publisher_name', 'developer_name', 'metacritic_score',
    'is_free', 'type'
]

df = pd.read_csv(input_file, usecols=ccols_to_keep)

df = df[(df['recommendations_total'] > 1000) | (df['metacritic_score'] > 0)].copy()

df = df.reset_index(drop=True)

# clear data
df['metacritic_score'] = df['metacritic_score'].fillna(0)
df['recommendations_total'] = df['recommendations_total'].fillna(0)
df['short_description'] = df['short_description'].fillna('')
df['genre_name'] = df['genre_name'].fillna('')

# merge in one text, create soup
def create_soup(x):
    genres = (str(x['genre_name']).replace(',', ' ') + " ") * 10
    dev = (str(x['developer_name']).replace(',', ' ') + " ")
    pub = str(x['publisher_name']).replace(',', '')
    description = str(x['short_description'])[:150]

    return f"{genres} {dev} {pub} {description}".lower()

df['soup'] = df.apply(create_soup, axis = 1)

# change texst to matrix numbers
tfidf = TfidfVectorizer(stop_words='english', max_features=5000, ngram_range=(1, 2)) # delete popular words like: 'and', 'the'
tfidf_matrix = tfidf.fit_transform(df['soup'])

# traning knn
model = NearestNeighbors(n_neighbors=10, metric='cosine', algorithm='brute')
model.fit(tfidf_matrix)


def get_recommendations(game_name, genre_weight=0.4, meta_weight=0.3, pop_weight=0.15):

    # try to find game in data base
    search_results = df[df['name'].str.contains(game_name, case=False, na=False)]

    if not search_results.empty:
        # known game
        idx = search_results.index[0]
        query_vector = tfidf_matrix[idx]
        base_name = df.iloc[idx]['name']
        main_genre = df.iloc[idx]['genre_name'].split(',')[0] if df.iloc[idx]['genre_name'] else ""
        is_cold_start = False
    else:
        # cold start
        query_vector = tfidf.transform([game_name.lower()])
        base_name = game_name
        main_genre = ""
        is_cold_start = True

    # find neighbors
    distances, indices = model.kneighbors(query_vector, n_neighbors=200)

    results = []

    dist_flat = distances.flatten()
    ind_flat = indices.flatten()

    for i in range(len(ind_flat)):
        res_idx = ind_flat[i]

        # skip game if is not i cold start
        if not is_cold_start and res_idx == idx:
            continue

        game_data = df.iloc[res_idx].copy()
        similarity = 1 - dist_flat[i]

        # penlaty for the same game name
        clean_base = base_name.lower().replace("the ", "").strip()
        first_word = clean_base.split()[0] if clean_base.split() else ""

        series_penalty = 0
        if len(first_word) > 3 and first_word in game_data['name'].lower():
            series_penalty = 0.4

        # Bonuses
        match_bonus = genre_weight if main_genre and main_genre in str(game_data['genre_name']) else 0
        meta_bonus = (game_data['metacritic_score'] / 100) * meta_weight
        pop_bonus = np.log10(game_data['recommendations_total'] + 1) * pop_weight

        game_data['final_score'] = similarity + match_bonus + meta_bonus + pop_bonus - series_penalty
        results.append(game_data)

    # results
    results_df = pd.DataFrame(results).sort_values(by='final_score', ascending=False)

    print(f"Top Recommended for {game_name} ({'BASE' if not is_cold_start else 'COLD-START'}):")
    for _, row in results_df.head(5).iterrows():
        print(f"- {row['name']} (Score: {row['final_score']:.2f})")

    return {
        "is_cold_start": is_cold_start,
        "base_game": base_name,
        "recommendations": results_df.head(15).to_dict(orient='records')
    }

def get_user_recommendations(game_names_list, genre_weight=0.4, meta_weight=0.3, pop_weight=0.15):
    valid_indices = []

    # find indexes of all user games
    for name in game_names_list:
        try:
            idx = df[df['name'].str.contains(name, case=False)].index[0]
            valid_indices.append(idx)
        except IndexError:
            print(f"game {name} dont exist")
    if not valid_indices:
        return  "There is no valid game in library"

    # get games vectors i calculate mean vector
    user_vectors = tfidf_matrix[valid_indices]
    user_profile_vector = np.asarray(user_vectors.mean(axis=0))

    # find closest neighbors for mean profile
    distances, indices = model.kneighbors(user_profile_vector, n_neighbors=250)

    results = []

    for i in range (len(distances.flatten())):
        res_idx = indices.flatten()[i]

        # skip games for user if game is in the list
        if res_idx in valid_indices:
            continue
        game_data = df.iloc[res_idx].copy()
        similarity = 1 - distances.flatten()[i]

        fav_genre = df.iloc[valid_indices[0]]['genre_name'].split(',')[0]
        match_bonus = genre_weight if fav_genre in str(game_data['genre_name']) else 0
        meta_bonus = (game_data['metacritic_score'] / 100) * meta_weight
        pop_bonus = np.log10(game_data['recommendations_total'] + 1) * pop_weight

        game_data['final_score'] = similarity + match_bonus + meta_bonus + pop_bonus
        results.append(game_data)

    results_df = pd.DataFrame(results).sort_values(by='final_score', ascending=False)
    final_list = results_df.head(15).to_dict(orient='records')

    for _, row in results_df.head(5).iterrows():
        print(f"- {row['name']} (Score: {row['final_score']:.2f})")

    return results_df.head(15)

# test
get_recommendations("crimson desert open world rpg fantasy action knights adventure")

my_library = ["The Witcher 3", "Skyrim", "Cyberpunk 2077"]
print("recomandation for my library")
get_user_recommendations(my_library)

# save dataframe
df.to_pickle('data/models/games_metadata.pkl')

# save tf-idf matrix
with open('data/models/tfidf_matrix.pkl', 'wb') as f:
    pickle.dump(tfidf_matrix, f)

# save knn model
with open('data/models/knn_model.pkl', 'wb') as f:
    pickle.dump(model, f)

# save the vectorizer
with open('data/models/tfidf_vectorizer.pkl', 'wb') as f:
    pickle.dump(tfidf, f)



