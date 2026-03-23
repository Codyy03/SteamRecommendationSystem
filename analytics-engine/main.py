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

def get_recommendations(game_name):
    # find game index
    try:
        idx = df[df['name'].str.contains(game_name, case=False)].index[0]
    except IndexError:
        return f"Game of name: {game_name} do not exist"

    # get game vector
    query_vector = tfidf_matrix[idx]

    # find 6 closest neighbors
    distances, indices = model.kneighbors(query_vector, n_neighbors=150)

    # temporary results array
    results = []
    for i in range(1, len(distances.flatten())):
        res_idx = indices.flatten()[i]
        game_data = df.iloc[res_idx].copy()

        # calculate final score
        similarity = 1 - distances.flatten()[i]

        main_genre = df.iloc[idx]['genre_name'].split(',')[0] if df.iloc[idx]['genre_name'] else ""
        match_bonus = 0.4 if main_genre in str(game_data['genre_name']) else 0

        meta_bonus = (game_data['metacritic_score'] / 100) * 0.3
        pop_bonus = np.log10(game_data['recommendations_total'] + 1) / 6

        game_data['final_score'] = similarity + match_bonus + meta_bonus + pop_bonus
        results.append(game_data)

    results_df = pd.DataFrame(results).sort_values(by='final_score', ascending=False)

    # show results
    print(f"Top Recommended for {game_name}:")
    for _, row in results_df.head(15).iterrows():
        print(f"- {row['name']} (Score: {row['final_score']:.2f})")


# test
get_recommendations("The Witcher 3")

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



