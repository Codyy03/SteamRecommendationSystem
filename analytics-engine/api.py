from fastapi import FastAPI, Query
import logic

app = FastAPI()

@app.get("/recommend")
def get_rec(
    query: str,
    genre_weight: float = 0.4,
    meta_weight: float = 0.3,
    pop_weight: float = 0.15
):
   result = logic.get_recommendations(query, genre_weight, meta_weight, pop_weight)

   return result

@app.get("/user_recommend")
def get_rec_user(
    game_names: str = Query(..., description="game names"),
    genre_weight: float = 0.4,
    meta_weight: float = 0.3,
    pop_weight: float = 0.15
):
    # split array of game names
    names_list = [n.strip() for n in game_names.split(",")]

    result = logic.get_user_recommendations(names_list, genre_weight, meta_weight, pop_weight)

    return result