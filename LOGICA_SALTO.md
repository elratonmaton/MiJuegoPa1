# PlayerController.cs — Lógica de Salto (1 min)

## En resumen

El salto se ejecuta **solo si el Player está en el suelo** y se aplica como **velocidad vertical instantánea** (no fuerza acumulada). Resultado: salto decidido, sin doble-salto accidental, sin "flotación".

## Flujo en 3 pasos

### 1. Detectar input (cada frame)
```csharp
var kb = Keyboard.current;
if (kb.spaceKey.wasPressedThisFrame && isGrounded) { ... }
```
- `wasPressedThisFrame` = `true` solo el frame exacto de la pulsación (evita auto-repeat).
- Si no se cumple **ambas** condiciones (Space + suelo), no salta.

### 2. Detectar suelo (OverlapCircle)
```csharp
isGrounded = Physics2D.OverlapCircle(
    groundCheck.position, groundCheckRadius, groundLayer) != null;
```
- Hay un GameObject hijo `GroundCheck` posicionado en `(0, -0.25, 0)` (justo bajo los pies).
- Un círculo de radio `0.15` busca colliders en esa zona.
- Si encuentra хотя бы uno → `isGrounded = true`.

### 3. Aplicar velocidad (no fuerza)
```csharp
rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
```
- **Importante:** se asigna `linearVelocity.y`, no `AddForce`. Esto da un salto consistente frame-a-frame.
- Se preserva la velocidad horizontal (`rb.linearVelocity.x`) para que puedas saltar mientras corres sin perder momentum.

## Por qué funciona así

| Decisión | Por qué |
|---|---|
| `wasPressedThisFrame` (no `isPressed`) | Evita salto sostenido infinito |
| `OverlapCircle` (no `OnCollisionStay`) | Detecta suelo sin requerir colisión exacta, robusto en bordes |
| Hijó `GroundCheck` separado | Permite ajustar el punto de detección sin tocar el sprite |
| `linearVelocity =` (no `AddForce`) | Salto predecible, mismo resultado cada vez |
| `Gravity Scale = 3.5` | Caída rápida, evita "sensación globo" |

## Valores numéricos

```csharp
moveSpeed      = 7     // velocidad horizontal
jumpForce      = 16    // velocidad vertical instantánea
gravityScale   = 3.5   // caída pesada (Hollow Knight-style)
groundCheckRadius = 0.15
```

**Altura máxima del salto:** `v² / (2g) = 16² / (2 × 9.81 × 3.5) ≈ 3.73` unidades.

---

**Resumen 1 línea:** *Input + suelo + velocidad instantánea = salto responsivo estilo Hollow Knight.*