export type Product = {
  id: string;
  ownerId?: string;
  categoryId?: string;
  name: string;
  sku?: string;
  description?: string;
  colour: string;
  price: number;
  discountedPrice?: number | null;
  imageUrl?: string;
  isActive?: boolean;
  isFeatured?: boolean;
  unitsSold?: number;
  createdUtc: string;
  updatedUtc?: string;
};
export type CreateProduct = { name: string; colour: string; price: number };
