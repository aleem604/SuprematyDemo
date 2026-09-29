import {api} from '../../api/client'; import type {CreateProduct,Product} from '../../types/product';
export const getProducts=async(colour?:string)=>(await api.get<Product[]>('/api/store/products',{params:{colour:colour||undefined}})).data;
export const createProduct=async(input:CreateProduct)=>(await api.post<Product>('/api/products',input)).data;