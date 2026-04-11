export async function getPythonRecomentationGamesByName(gameName: string, genre: number, met: number, pop: number, howManyGames: number, series_penalty: number) {
    const response = await fetch(`https://localhost:7179/api/PythonRecommendation/pythonRecommendation?gameName=${gameName}&genre=${genre}&met=${met}&pop=${pop}&howManyGames=${howManyGames}&series_penalty=${series_penalty}`)

    if (!response.ok) throw new Error("Error when downloading games");

    return response.json();
}

export async function getPythonUserRecomentationGamesByName(gameName: string, genre: number, met: number, pop: number, howManyGames: number) {
    const response = await fetch(`https://localhost:7179/api/PythonRecommendation/pythonUserRecommendation?gameName=${gameName}&genre=${genre}&met=${met}&pop=${pop}&howManyGames=${howManyGames}`)

    if (!response.ok) throw new Error("Error when downloading games");

    return response.json();
}