import api from "./api";

export async function getUserLibrary() {
    const res = await api.get("/api/userLibrary/userLibrary");
    return res.data;
}