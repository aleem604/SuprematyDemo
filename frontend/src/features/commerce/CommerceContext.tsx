import {
  createContext,
  useContext,
  useMemo,
  useState,
  type ReactNode,
} from 'react';
import type {Product} from '../../types/product';

export type CartItem={product:Product;quantity:number};
type Ctx={
  cart:CartItem[]; wishlist:string[]; favorites:string[]; coupon:string;
  cartCount:number; wishlistCount:number; favoriteCount:number;
  add:(p:Product)=>void; setQty:(id:string,q:number)=>void; increment:(id:string)=>void; decrement:(id:string)=>void; remove:(id:string)=>void;
  isInCart:(id:string)=>boolean; quantityOf:(id:string)=>number;
  toggleWishlist:(id:string)=>void; isWishlisted:(id:string)=>boolean;
  toggleFavorite:(id:string)=>void; isFavorite:(id:string)=>boolean;
  setCoupon:(v:string)=>void; clear:()=>void;
};
const C=createContext<Ctx|null>(null);
const read=<T,>(k:string,d:T):T=>{try{const raw=localStorage.getItem(k);return raw?JSON.parse(raw) as T:d}catch{return d}};
const persist=(k:string,v:unknown)=>localStorage.setItem(k,JSON.stringify(v));
const seeded=(n:number)=>Array.from({length:n},(_,i)=>`10000000-0000-0000-0000-${String(i+1).padStart(12,'0')}`);

export function CommerceProvider({children}:{children:ReactNode}){
  const [cart,setCart]=useState<CartItem[]>(()=>read('cart',[]));
  const [wishlist,setWishlist]=useState<string[]>(()=>read('wishlist',seeded(8).slice(3)));
  const [favorites,setFavorites]=useState<string[]>(()=>read('favorites',seeded(6)));
  const [coupon,setCouponState]=useState('');

  const value=useMemo<Ctx>(()=>{
    const updateCart=(fn:(current:CartItem[])=>CartItem[])=>setCart(current=>{const next=fn(current);persist('cart',next);return next});
    return {
      cart,wishlist,favorites,coupon,
      cartCount:cart.reduce((sum,x)=>sum+x.quantity,0),
      wishlistCount:wishlist.length,favoriteCount:favorites.length,
      add:p=>updateCart(c=>c.some(x=>x.product.id===p.id)?c.map(x=>x.product.id===p.id?{...x,quantity:x.quantity+1}:x):[...c,{product:p,quantity:1}]),
      setQty:(id,q)=>updateCart(c=>q<=0?c.filter(x=>x.product.id!==id):c.map(x=>x.product.id===id?{...x,quantity:q}:x)),
      increment:id=>updateCart(c=>c.map(x=>x.product.id===id?{...x,quantity:x.quantity+1}:x)),
      decrement:id=>updateCart(c=>c.flatMap(x=>x.product.id!==id?[x]:x.quantity<=1?[]:[{...x,quantity:x.quantity-1}])),
      remove:id=>updateCart(c=>c.filter(x=>x.product.id!==id)),
      isInCart:id=>cart.some(x=>x.product.id===id), quantityOf:id=>cart.find(x=>x.product.id===id)?.quantity??0,
      toggleWishlist:id=>setWishlist(c=>{const n=c.includes(id)?c.filter(x=>x!==id):[...c,id];persist('wishlist',n);return n}),
      isWishlisted:id=>wishlist.includes(id),
      toggleFavorite:id=>setFavorites(c=>{const n=c.includes(id)?c.filter(x=>x!==id):[...c,id];persist('favorites',n);return n}),
      isFavorite:id=>favorites.includes(id),
      setCoupon:setCouponState,
      clear:()=>{setCart([]);persist('cart',[])},
    };
  },[cart,wishlist,favorites,coupon]);
  return <C.Provider value={value}>{children}</C.Provider>;
}
export const useCommerce=()=>{const v=useContext(C);if(!v)throw new Error('CommerceProvider missing');return v};
