# Architecture
```mermaid
flowchart LR
 UI[React SPA] --> GW[API Gateway / BFF]
 GW --> IDP[OIDC Identity Provider]
 GW --> P[SuprematyDemo Products API]
 GW --> O[Orders Service]
 GW --> PAY[Payments Service]
 P --> PDB[(Products SQLite / Production RDBMS)]
 O --> ODB[(Orders DB)]
 PAY --> PAYDB[(Payments DB)]
 P -- ProductCreated --> BUS[(Event Bus)]
 O -- OrderCreated --> BUS
 PAY -- PaymentCompleted --> BUS
 BUS --> O
 BUS --> PAY
 BUS --> N[Notification Service]
```
Clean Architecture dependency rule inside Products: `API -> Application <- Infrastructure`, with both Application and Infrastructure depending on Domain abstractions/entities as appropriate. For production event delivery, use a transactional outbox and idempotent consumers.
