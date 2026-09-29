# SuprematyDemo Enterprise Commerce SPA

Clean Architecture .NET 10 + EF Core/SQLite backend with a Vite/React/TypeScript/Tailwind frontend.

## Commerce experience
- Public dashboard/home with header Login and Sign up actions
- Public shared product detail route
- Authentication-aware route guards; protected links redirect to Login and return to the requested route after authentication
- Product catalog, search, cart, quantity controls, remove from cart
- Favorites and wishlist with dedicated views
- Checkout with delivery form, Card/Cash-on-delivery selection, WELCOME10 coupon, discount and order summary
- Order confirmation/history and account view
- Login, signup, forgot/reset password
- Persistent demo cart/wishlist/favorites/order state in browser storage

> Card payment is intentionally provider-ready rather than collecting/storing raw card numbers. For production, integrate a PCI-compliant hosted payment UI (e.g. Stripe/Adyen) and persist carts/orders/favorites/wishlists server-side.

## Demo user
`demo@suprematy.local` / `Demo123!`

## Run
Backend: `cd backend/src/SuprematyDemo.Api && dotnet run`
Frontend: `cd frontend && npm install && npm run dev`
