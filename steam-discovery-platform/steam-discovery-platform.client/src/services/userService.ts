import api from "./api";

export async function getMe() {
    const res = await api.get("/api/Users/me");
    return res.data;
}