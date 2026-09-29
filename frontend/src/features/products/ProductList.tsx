import { useState } from "react";
import { useProducts } from "./useProducts";
export function ProductList() {
  const [colour, setColour] = useState("");
  const { data = [], isLoading, isError } = useProducts(colour);
  return (
    <section className="card">
      <div className="toolbar">
        <div>
          <h2>Products</h2>
          <p>Server-state cached with TanStack Query.</p>
        </div>
        <input
          aria-label="Filter by colour"
          placeholder="Filter colour…"
          value={colour}
          onChange={(e) => setColour(e.target.value)}
        />
      </div>
      {isLoading ? (
        <p>Loading…</p>
      ) : isError ? (
        <p className="error">Could not load products. Add a JWT token first.</p>
      ) : (
        <div className="grid">
          {data.map((p) => (
            <article key={p.id}>
              <span className="badge">{p.colour}</span>
              <h3>{p.name}</h3>
              <strong>${p.price.toFixed(2)}</strong>
            </article>
          ))}
          {!data.length && <p>No products found.</p>}
        </div>
      )}
    </section>
  );
}
