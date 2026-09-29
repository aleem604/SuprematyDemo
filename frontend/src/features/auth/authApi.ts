import { api } from "../../api/client";
export type User = { id: string; email: string; displayName: string };
export type AuthResponse = {
  accessToken: string;
  expiresUtc: string;
  user: User;
};
export const authApi = {
  login: async (body: { email: string; password: string }) =>
    (await api.post<AuthResponse>("/api/auth/login", body)).data,
  signup: async (body: {
    email: string;
    password: string;
    displayName: string;
  }) => (await api.post<AuthResponse>("/api/auth/signup", body)).data,
  forgot: async (email: string) =>
    (
      await api.post<{ message: string; developmentResetToken?: string }>(
        "/api/auth/forgot-password",
        { email },
      )
    ).data,
  reset: async (body: { email: string; token: string; newPassword: string }) =>
    api.post("/api/auth/reset-password", body),
  me: async () => (await api.get<User>("/api/auth/me")).data,
};
