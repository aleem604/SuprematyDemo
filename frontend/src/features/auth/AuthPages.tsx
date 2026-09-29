import { FormEvent, useState, ReactNode } from "react";
import {
  Link,
  useLocation,
  useNavigate,
  useSearchParams,
} from "react-router-dom";
import { useAuth } from "./AuthContext";
import { authApi } from "./authApi";
const message = (e: any) =>
  e?.response?.data?.detail || e?.message || "Request failed";
function Shell({
  title,
  subtitle,
  children,
}: {
  title: string;
  subtitle: string;
  children: ReactNode;
}) {
  return (
    <main className="grid min-h-screen bg-slate-950 lg:grid-cols-2">
      <section className="hidden p-12 text-white lg:flex lg:flex-col lg:justify-between">
        <Link className="text-2xl font-black" to="/">
          Suprematy
        </Link>
        <div>
          <span className="rounded-full bg-indigo-500/20 px-4 py-2 text-sm font-bold text-indigo-300">
            SECURE COMMERCE
          </span>
          <h2 className="mt-6 max-w-lg text-5xl font-black">
            Everything you love, one account away.
          </h2>
          <p className="mt-5 max-w-md text-slate-400">
            Manage products, favorites, wishlists, cart, checkout and orders
            from one polished experience.
          </p>
        </div>
        <p className="text-sm text-slate-500">
          React · Vite · Tailwind CSS · .NET
        </p>
      </section>
      <section className="flex items-center justify-center bg-white px-6 py-12">
        <div className="w-full max-w-md">
          <Link className="mb-10 inline-block font-black lg:hidden" to="/">
            ← Suprematy
          </Link>
          <h1 className="text-4xl font-black">{title}</h1>
          <p className="mt-2 text-slate-500">{subtitle}</p>
          {children}
        </div>
      </section>
    </main>
  );
}
const Field = ({ label, ...p }: any) => (
  <label className="block">
    <span className="label">{label}</span>
    <input className="input" {...p} />
  </label>
);
export function LoginPage() {
  const { login } = useAuth(),
    nav = useNavigate(),
    loc = useLocation();
  const [email, setEmail] = useState("demo@suprematy.local"),
    [password, setPassword] = useState("Demo123!"),
    [error, setError] = useState(""),
    [busy, setBusy] = useState(false);
  async function submit(e: FormEvent) {
    e.preventDefault();
    setBusy(true);
    setError("");
    try {
      await login(email, password);
      nav((loc.state as any)?.from || "/products", { replace: true });
    } catch (x) {
      setError(message(x));
    } finally {
      setBusy(false);
    }
  }
  return (
    <Shell title="Welcome back" subtitle="Sign in to continue shopping.">
      <form className="mt-8 space-y-5" onSubmit={submit}>
        <Field
          label="Email address"
          value={email}
          onChange={(e: any) => setEmail(e.target.value)}
          type="email"
          required
        />
        <Field
          label="Password"
          value={password}
          onChange={(e: any) => setPassword(e.target.value)}
          type="password"
          required
        />
        <div className="flex justify-end">
          <Link
            className="text-sm font-bold text-indigo-600"
            to="/forgot-password"
          >
            Forgot password?
          </Link>
        </div>
        <button disabled={busy} className="btn-primary w-full">
          {busy ? "Signing in…" : "Sign in"}
        </button>
        {error && <p className="error-text">{error}</p>}
      </form>
      <div className="mt-5 rounded-xl bg-indigo-50 p-4 text-sm text-indigo-800">
        <strong>Demo account</strong>
        <br />
        demo@suprematy.local / Demo123!
      </div>
      <p className="mt-7 text-center text-sm text-slate-500">
        New here?{" "}
        <Link className="font-bold text-indigo-600" to="/signup">
          Create an account
        </Link>
      </p>
    </Shell>
  );
}
export function SignupPage() {
  const { signup } = useAuth(),
    nav = useNavigate();
  const [name, setName] = useState(""),
    [email, setEmail] = useState(""),
    [password, setPassword] = useState(""),
    [error, setError] = useState("");
  async function submit(e: FormEvent) {
    e.preventDefault();
    try {
      await signup(email, password, name);
      nav("/products");
    } catch (x) {
      setError(message(x));
    }
  }
  return (
    <Shell
      title="Create your account"
      subtitle="Join Suprematy in less than a minute."
    >
      <form className="mt-8 space-y-5" onSubmit={submit}>
        <Field
          label="Full name"
          value={name}
          onChange={(e: any) => setName(e.target.value)}
          required
        />
        <Field
          label="Email address"
          value={email}
          onChange={(e: any) => setEmail(e.target.value)}
          type="email"
          required
        />
        <Field
          label="Password"
          value={password}
          onChange={(e: any) => setPassword(e.target.value)}
          type="password"
          minLength={8}
          required
        />
        <button className="btn-primary w-full">Create account</button>
        {error && <p className="error-text">{error}</p>}
      </form>
      <p className="mt-7 text-center text-sm text-slate-500">
        Already have an account?{" "}
        <Link className="font-bold text-indigo-600" to="/login">
          Sign in
        </Link>
      </p>
    </Shell>
  );
}
export function ForgotPasswordPage() {
  const [email, setEmail] = useState(""),
    [info, setInfo] = useState<any>(null);
  async function submit(e: FormEvent) {
    e.preventDefault();
    setInfo(await authApi.forgot(email));
  }
  return (
    <Shell title="Forgot password?" subtitle="We'll help you regain access.">
      <form className="mt-8 space-y-5" onSubmit={submit}>
        <Field
          label="Email address"
          value={email}
          onChange={(e: any) => setEmail(e.target.value)}
          type="email"
          required
        />
        <button className="btn-primary w-full">Request reset</button>
      </form>
      {info && (
        <div className="mt-5 rounded-xl bg-emerald-50 p-4 text-sm text-emerald-800">
          <p>{info.message}</p>
          {info.developmentResetToken && (
            <Link
              className="mt-2 inline-block font-bold"
              to={`/reset-password?email=${encodeURIComponent(email)}&token=${info.developmentResetToken}`}
            >
              Continue with development token →
            </Link>
          )}
        </div>
      )}
      <Link className="mt-7 inline-block font-bold text-indigo-600" to="/login">
        ← Back to sign in
      </Link>
    </Shell>
  );
}
export function ResetPasswordPage() {
  const [q] = useSearchParams(),
    nav = useNavigate();
  const [email, setEmail] = useState(q.get("email") || ""),
    [token, setToken] = useState(q.get("token") || ""),
    [password, setPassword] = useState(""),
    [error, setError] = useState("");
  async function submit(e: FormEvent) {
    e.preventDefault();
    try {
      await authApi.reset({ email, token, newPassword: password });
      nav("/login");
    } catch (x) {
      setError(message(x));
    }
  }
  return (
    <Shell
      title="Set a new password"
      subtitle="Choose a strong password for your account."
    >
      <form className="mt-8 space-y-5" onSubmit={submit}>
        <Field
          label="Email"
          value={email}
          onChange={(e: any) => setEmail(e.target.value)}
          type="email"
          required
        />
        <Field
          label="Reset token"
          value={token}
          onChange={(e: any) => setToken(e.target.value)}
          required
        />
        <Field
          label="New password"
          value={password}
          onChange={(e: any) => setPassword(e.target.value)}
          type="password"
          minLength={8}
          required
        />
        <button className="btn-primary w-full">Reset password</button>
        {error && <p className="error-text">{error}</p>}
      </form>
    </Shell>
  );
}
