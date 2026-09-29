export interface AdminProduct {
  id: string;
  name: string;
  sku: string;
  colour: string;
  price: number;
  discountedPrice?: number | null;
  description?: string;
  imageUrl?: string | null;
  categoryId?: string | null;
  isActive: boolean;
  isFeatured: boolean;
}

export interface InventoryItem {
  id: string;
  productId: string;
  quantityOnHand: number;
  reorderLevel: number;
  warehouse: string;
}

export interface Category {
  id: string;
  name: string;
  slug: string;
  isActive: boolean;
}

export interface Coupon {
  id: string;
  code: string;
  discountPercent: number;
  minimumOrder?: number | null;
  isActive: boolean;
  expiresUtc?: string | null;
}

export interface AdminOrder {
  id: string;
  number: string;
  customerName: string;
  customerEmail: string;
  createdUtc: string;
  total: number;
  status: string;
  paymentMethod: string;
  paymentStatus: string;
}

export interface AdminCustomer {
  id: string;
  displayName: string;
  email: string;
  createdUtc: string;
}

export interface AdminDashboardSummary {
  products: number;
  orders: number;
  customers: number;
  lowStock: number;
  revenue: number;
}
