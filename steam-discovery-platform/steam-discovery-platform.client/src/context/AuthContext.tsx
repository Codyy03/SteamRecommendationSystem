import { createContext, useState, useEffect, useCallback } from "react";
import { jwtDecode } from "jwt-decode";
import api from "../services/api";

export interface JwtPayload {
    name?: string;
    unique_name?: string;
    email?: string;
    exp?: number;
    role?: string;
    [key: string]: unknown;
}

interface AuthContextType {
    userName: string | null;
    userRole: string | null;
    loading: boolean;
    logout: () => void;
    login: (accessToken: string, refreshToken: string) => void;
    refreshAccessToken: () => Promise<void>;
}

// eslint-disable-next-line react-refresh/only-export-components
export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [userName, setUserName] = useState<string | null>(null);
    const [userRole, setUserRole] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    const logout = useCallback(() => {
        localStorage.removeItem("token");
        localStorage.removeItem("refreshToken");
        setUserName(null);
        setUserRole(null);
    }, []);

    const setDecodedUser = useCallback((decoded: JwtPayload) => {
        const name =
            decoded.name ??
            decoded.unique_name ??
            decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] ??
            null;

        const role =
            decoded.role ??
            decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ??
            null;

        setUserName(typeof name === 'string' ? name : null);
        setUserRole(typeof role === 'string' ? role : null);
    }, []);

    const refreshAccessToken = useCallback(async () => {
        const refreshToken = localStorage.getItem("refreshToken");
        if (!refreshToken) {
            logout();
            return;
        }

        try {
            const res = await api.post("/api/Auth/refresh", { refreshToken });
            const { accessToken, refreshToken: newRefreshToken } = res.data;

            localStorage.setItem("token", accessToken);
            localStorage.setItem("refreshToken", newRefreshToken);

            const decoded = jwtDecode<JwtPayload>(accessToken);
            setDecodedUser(decoded);
        } catch (err) {
            console.error("Refresh failed", err);
            logout();
        }
    }, [logout, setDecodedUser]);

    const login = (accessToken: string, refreshToken: string) => {
        localStorage.setItem("token", accessToken);
        localStorage.setItem("refreshToken", refreshToken);
        try {
            const decoded = jwtDecode<JwtPayload>(accessToken);
            setDecodedUser(decoded);
        } catch {
            logout();
        }
    };

    useEffect(() => {
        const loadUser = async () => {
            const token = localStorage.getItem("token");
            if (token) {
                try {
                    const decoded = jwtDecode<JwtPayload>(token);

                    if (decoded.exp && decoded.exp * 1000 < Date.now()) {
                        await refreshAccessToken();
                    } else {
                        setDecodedUser(decoded);
                    }
                } catch {
                    logout();
                }
            } else {
                setUserName(null);
                setUserRole(null);
            }
            setLoading(false);
        };

        loadUser();
    }, [refreshAccessToken, logout, setDecodedUser]);

    return (
        <AuthContext.Provider value={{ userName, userRole, loading, logout, login, refreshAccessToken }}>
            {children}
        </AuthContext.Provider>
    );
};
