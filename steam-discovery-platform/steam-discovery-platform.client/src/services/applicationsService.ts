export async function getGames() {
    const response = await fetch("https://localhost:7179/api/applications")

    if (!response.ok) throw new Error("Error when downloading games");

    return response.json();
}