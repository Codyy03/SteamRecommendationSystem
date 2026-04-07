import api from "./api";

export async function getMe() {
    const res = await api.get("/api/Users/me");
    return res.data;
}

export async function updateUser(data: { userName: string; email: string }) {
    const res = await api.put("/api/Users/update", data);
    return res.data;
}