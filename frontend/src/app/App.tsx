import type { ReactNode } from "react";
import { Navigate, Route, Routes, useLocation } from "react-router-dom";
import { useAuth } from "../features/auth/AuthContext";
import {
  LoginPage,
  SignupPage,
  ForgotPasswordPage,
  ResetPasswordPage,
} from "../features/auth/AuthPages";
import { Layout } from "../components/Layout";
import {
  AdminDashboard,
  AdminProducts,
  AdminOrders,
  AdminInventory,
  AdminCategories,
  AdminCoupons,
  AdminCustomers,
} from "../pages/AdminPages";
import {
  HomePage,
  ProductsPage,
  ProductDetailPage,
  CartPage,
  CheckoutPage,
  WishlistPage,
  FavoritesPage,
  OrdersPage,
  AccountPage,
  NotFoundPage,
} from "../pages/CommercePages";

function Protected({ children }: { children: ReactNode }) {
  const { token } = useAuth();
  const location = useLocation();
  return token ? (
    <>{children}</>
  ) : (
    <Navigate to="/login" replace state={{ from: location.pathname }} />
  );
}
export default function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<HomePage />} />
        <Route path="/product/:id" element={<ProductDetailPage />} />
        <Route
          path="/products"
          element={
            <Protected>
              <ProductsPage />
            </Protected>
          }
        />
        <Route
          path="/cart"
          element={
            <Protected>
              <CartPage />
            </Protected>
          }
        />
        <Route
          path="/checkout"
          element={
            <Protected>
              <CheckoutPage />
            </Protected>
          }
        />
        <Route
          path="/wishlist"
          element={
            <Protected>
              <WishlistPage />
            </Protected>
          }
        />
        <Route
          path="/favorites"
          element={
            <Protected>
              <FavoritesPage />
            </Protected>
          }
        />
        <Route
          path="/orders"
          element={
            <Protected>
              <OrdersPage />
            </Protected>
          }
        />
        <Route
          path="/account"
          element={
            <Protected>
              <AccountPage />
            </Protected>
          }
        />
        <Route
          path="/admin"
          element={
            <Protected>
              <AdminDashboard />
            </Protected>
          }
        />
        <Route
          path="/admin/products"
          element={
            <Protected>
              <AdminProducts />
            </Protected>
          }
        />
        <Route
          path="/admin/orders"
          element={
            <Protected>
              <AdminOrders />
            </Protected>
          }
        />
        <Route
          path="/admin/inventory"
          element={
            <Protected>
              <AdminInventory />
            </Protected>
          }
        />
        <Route
          path="/admin/categories"
          element={
            <Protected>
              <AdminCategories />
            </Protected>
          }
        />
        <Route
          path="/admin/coupons"
          element={
            <Protected>
              <AdminCoupons />
            </Protected>
          }
        />
        <Route
          path="/admin/customers"
          element={
            <Protected>
              <AdminCustomers />
            </Protected>
          }
        />
      </Route>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/signup" element={<SignupPage />} />
      <Route path="/forgot-password" element={<ForgotPasswordPage />} />
      <Route path="/reset-password" element={<ResetPasswordPage />} />
      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
}
