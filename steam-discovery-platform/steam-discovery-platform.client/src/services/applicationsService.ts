import api from "./api";

export async function getGames() {
    const response = await fetch("https://localhost:7179/api/applications/getGames");

    if (!response.ok) throw new Error("Error when downloading games");

    return response.json();
}

export async function getGamesByGenre(genre: string) {
    const response = await fetch(`https://localhost:7179/api/applications/getGamesByGenre?genre=${genre}`);

    if (!response.ok) throw new Error("Error when downloading games");

    return response.json();
}

export async function getGameDetails(id: number) {
    const res = await api.get(`/api/applications/getGameDetails?id=${id}`);
    return res.data;
}