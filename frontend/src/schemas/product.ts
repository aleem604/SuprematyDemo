import { z } from "zod";
export const productSchema = z.object({
  name: z.string().trim().min(2).max(200),
  colour: z.string().trim().min(2).max(50),
  price: z.coerce.number().min(0),
});
export type ProductForm = z.infer<typeof productSchema>;
