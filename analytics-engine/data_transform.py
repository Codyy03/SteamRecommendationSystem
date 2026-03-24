import pandas as pd

# load names dictionary
genres_dict = pd.read_csv('data/raw/genres.csv').set_index('id')['name'].to_dict()
devs_dict = pd.read_csv('data/raw/developers.csv').set_index('id')['name'].to_dict()
pubs_dict = pd.read_csv('data/raw/publishers.csv').set_index('id')['name'].to_dict()

# load helper tables
print("Przetwarzanie tabel relacyjnych...")

app_genres = pd.read_csv('data/raw/application_genres.csv')
app_genres['genre_name'] = app_genres['genre_id'].map(genres_dict)
genres_grouped = app_genres.groupby('appid')['genre_name'].apply(lambda x: ', '.join(x.dropna())).reset_index()

app_devs = pd.read_csv('data/raw/application_developers.csv')
app_devs['developer_name'] = app_devs['developer_id'].map(devs_dict)
devs_grouped = app_devs.groupby('appid')['developer_name'].apply(lambda x: ', '.join(x.dropna())).reset_index()

app_pubs = pd.read_csv('data/raw/application_publishers.csv')
app_pubs['publisher_name'] = app_pubs['publisher_id'].map(pubs_dict)
pubs_grouped = app_pubs.groupby('appid')['publisher_name'].apply(lambda x: ', '.join(x.dropna())).reset_index()

# load main tables
apps_cols = [
    'appid', 'name', 'short_description', 'type',
    'metacritic_score', 'recommendations_total', 'is_free'
]
apps = pd.read_csv('data/raw/applications.csv', usecols=apps_cols)

# filters, only games
apps = apps[apps['type'] == 'game'].copy()

# connect every table in one big dataframe
print("Scalanie danych...")
final_df = apps.merge(genres_grouped, on='appid', how='left')
final_df = final_df.merge(devs_grouped, on='appid', how='left')
final_df = final_df.merge(pubs_grouped, on='appid', how='left')

# cleaning
final_df['genre_name'] = final_df['genre_name'].fillna('')
final_df['developer_name'] = final_df['developer_name'].fillna('Unknown')
final_df['publisher_name'] = final_df['publisher_name'].fillna('Unknown')
final_df['short_description'] = final_df['short_description'].fillna('')

# save result
final_df.to_csv('data/steam_games_final.csv', index=False)
print(f"Sukces! Scalono dane dla {len(final_df)} gier.")