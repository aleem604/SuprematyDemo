import { Link, NavLink, Outlet } from "react-router-dom";
import { useAuth } from "../features/auth/AuthContext";
import { useCommerce } from "../features/commerce/CommerceContext";
export function Layout() {
  const { token, user, logout } = useAuth();
  const { cartCount, wishlistCount, favoriteCount } = useCommerce();
  const nav = ({ isActive }: { isActive: boolean }) =>
    `text-sm font-semibold transition ${isActive ? "text-indigo-600" : "text-slate-600 hover:text-slate-950"}`;
  return (
    <div className="flex min-h-screen flex-col">
      <header className="sticky top-0 z-40 border-b border-slate-200/80 bg-white/90 backdrop-blur">
        <div className="mx-auto flex h-16 max-w-7xl items-center justify-between px-4 sm:px-6">
          <Link to="/" className="flex items-center gap-2 text-xl font-black">
            <span className="grid h-9 w-9 place-items-center rounded-xl bg-indigo-600 text-white">
              S
            </span>
            Suprematy
          </Link>
          <nav className="hidden items-center gap-7 md:flex">
            <NavLink className={nav} to="/">
              Home
            </NavLink>
            <NavLink className={nav} to="/products">
              Products
            </NavLink>
            {token && (
              <>
                <NavLink className={nav} to="/favorites">
                  Favorites{" "}
                  <span className="ml-1 rounded-full bg-slate-100 px-1.5 text-xs">
                    {favoriteCount}
                  </span>
                </NavLink>
                <NavLink className={nav} to="/wishlist">
                  Wishlist{" "}
                  <span className="ml-1 rounded-full bg-slate-100 px-1.5 text-xs">
                    {wishlistCount}
                  </span>
                </NavLink>
                <NavLink className={nav} to="/orders">
                  Orders
                </NavLink>
                <NavLink className={nav} to="/admin">
                  Admin
                </NavLink>
              </>
            )}
          </nav>
          <div className="flex items-center gap-2">
            {token ? (
              <>
                <Link
                  className="relative rounded-xl border px-3 py-2 text-sm font-semibold"
                  to="/cart"
                >
                  Cart{" "}
                  <span className="ml-1 rounded-full bg-indigo-600 px-1.5 text-xs text-white">
                    {cartCount}
                  </span>
                </Link>
                <Link
                  className="hidden text-sm font-semibold sm:block"
                  to="/account"
                >
                  {user?.displayName}
                </Link>
                <button className="btn-secondary py-2" onClick={logout}>
                  Logout
                </button>
              </>
            ) : (
              <>
                <Link className="btn-secondary py-2" to="/login">
                  Login
                </Link>
                <Link className="btn-primary py-2" to="/signup">
                  Sign up
                </Link>
              </>
            )}
          </div>
        </div>
      </header>
      <div className="flex-1">
        <Outlet />
      </div>
      <footer className="mt-20 border-t bg-slate-950 text-slate-300">
        <div className="mx-auto grid max-w-7xl gap-8 px-6 py-12 md:grid-cols-3">
          <div>
            <h3 className="text-lg font-bold text-white">Suprematy</h3>
            <p className="mt-3 text-sm">
              A production-style commerce demo built with .NET, React, Vite and
              Tailwind CSS.
            </p>
          </div>
          <div>
            <h4 className="font-bold text-white">Shop</h4>
            <div className="mt-3 grid gap-2 text-sm">
              <Link to="/products">Products</Link>
              <Link to="/wishlist">Wishlist</Link>
              <Link to="/favorites">Favorites</Link>
            </div>
          </div>
          <div>
            <h4 className="font-bold text-white">Account</h4>
            <div className="mt-3 grid gap-2 text-sm">
              <Link to="/orders">Orders</Link>
              <Link to="/account">Profile</Link>
            </div>
          </div>
        </div>
      </footer>
    </div>
  );
}
