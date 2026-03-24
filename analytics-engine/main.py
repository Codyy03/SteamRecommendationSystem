import pandas as pd
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



