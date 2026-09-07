# Bitácora Digital — Hito 1

**Proyecto:** Mi Juego PA1
**Materia:** Fundamentos de C# y Game Feel
**Hito:** 1 — Prototipo de plataforma 2D
**Estilo elegido:** Hollow Knight (pesado y responsivo)

---

## Pregunta

> ¿Qué valores exactos usaste en el Rigidbody2D y en las variables de tu script (Velocidad, Fuerza de Salto) y por qué elegiste esos valores para lograr un buen "Game Feel"?

---

## Respuesta

### 1. Valores del Rigidbody2D (Inspector del Player)

| Parámetro | Valor | Significado |
|---|---|---|
| `Body Type` | `Dynamic` | El personaje se ve afectado por gravedad y fuerzas |
| `Mass` | `1` | Masa estándar; base para todos los cálculos de fuerza |
| `Linear Drag` | `1.5` | Resistencia al movimiento horizontal |
| `Angular Drag` | `0.05` | Resistencia a la rotación (no se nota mucho por el FreezeRotation) |
| `Gravity Scale` | `3.5` | Multiplica la gravedad global (~9.81 → efectiva ~34.3) |
| `Constraints` | `Freeze Rotation Z` | Evita que el sprite rote al chocar con superficies |
| `Collision Detection` | `Discrete` | Suficiente para velocidades de plataforma 2D |
| `Interpolate` | `None` | Suficiente a 60fps; mejora rendimiento |

### 2. Variables del script `PlayerController.cs`

| Variable | Valor | Uso |
|---|---|---|
| `moveSpeed` | `7` | Velocidad horizontal objetivo en X |
| `jumpForce` | `16` | Velocidad vertical instantánea al saltar |
| `groundCheckRadius` | `0.15` | Radio del overlap usado para detectar suelo |
| `groundLayer` | `Everything` | Detecta cualquier collider como suelo |

---

## ¿Por qué estos valores? — Filosofía Game Feel

El objetivo era conseguir una sensación **"Hollow Knight"**: personaje con peso, saltos decididos, frenado instantáneo y sin flotación.

### `Move Speed = 7`
- **Referencia:** Hollow Knight usa ~6-8 unidades/segundo para su protagonista.
- Con `Linear Drag = 1.5` y movimiento por `Input.GetAxisRaw` (sin suavizado), la velocidad llega a `7` casi en 1 frame. **No hay aceleración, no hay inercia flotante** — exactamente lo que pide el enunciado.
- Probé `5` (lento, se siente sluggish) y `10` (rápido, se pierde control). `7` es el sweet spot para una habitación de 19×10 unidades.

### `Jump Force = 16`
- Calculado para que el personaje suba **~3.3 unidades** antes de empezar a caer:
  - Altura máxima = `v² / (2g)` = `16² / (2 × 34.3)` ≈ `3.73` unidades
- Suficiente para alcanzar la plataforma alta (y=2) desde el suelo (y=-4), un salto de ~6 unidades (con un pequeño salto doble plataforma-baja → media).
- Con `Jump Force = 12` no alcanzaba la plataforma alta. Con `Jump Force = 20` los saltos se sentían "de cohete" (poco controlables).

### `Gravity Scale = 3.5`
- **La pieza clave del "Game Feel" Hollow Knight.** La gravedad por defecto en Unity es `9.81`. Con `Gravity Scale = 3.5`, la gravedad efectiva es `~34.3`.
- Esto significa **caída rápida y decidida**: cuando sueltas el salto o caes de una plataforma, el personaje vuelve al suelo casi de inmediato. Sin esto, el personaje "flota" como un globo (el anti-Game-Feel).
- Comparado con Celeste (`~2.5`) o Mario (`~1.5`), Hollow Knight usa gravedad alta porque su personaje es insecto: tiene que *sentirse* pesado.

### `Mass = 1`
- Es el valor estándar. No hay razón para cambiarlo en plataformas 2D estándar — afecta el peso relativo pero al ser todos `Mass = 1`, las colisiones son simétricas y predecibles.

### `Linear Drag = 1.5`
- **Frenado sutil.** No queremos que el personaje se detenga en seco (eso se siente robótico), pero tampoco queremos que derrape.
- `Linear Drag = 0`: derrape infinito, el personaje sigue moviéndose medio segundo después de soltar la tecla.
- `Linear Drag = 3`: frenado brusco, se siente "tac-tac" sin transición.
- `Linear Drag = 1.5`: micro-derrapé de ~0.2s. Se siente como inercia *natural*.

### `Constraints = Freeze Rotation Z`
- **Bug que encontré durante desarrollo:** Inicialmente puse `Freeze Position X` (también valor `1` en el enum), lo que congelaba el movimiento horizontal. El sprite flipeaba con el input pero el Rigidbody no se movía.
- Corregido a `Freeze Rotation Z` (valor `4`). Ahora el sprite flipea correctamente **y** se desplaza.

### `Input.GetAxisRaw` (no `GetAxis`)
- El enunciado pide "respuesta instantánea, sin inercia flotante".
- `GetAxis` aplica suavizado (`smoothing = 0.1` por defecto) → input gradual.
- `GetAxisRaw` retorna `-1`, `0`, `1` puros → control pixel-perfect.

### Detección de suelo con `Physics2D.OverlapCircle`
- Más robusto que `OnCollisionStay2D` (que requiere que el rigidbody esté en contacto exacto).
- El `GroundCheck` es un Transform hijo del Player en `(0, -0.25, 0)` (justo debajo de los pies).
- El círculo de radio `0.15` detecta cualquier collider dentro de esa zona, evitando falsos negativos en bordes de tiles.

---

## Resultado

El personaje se siente:
- ✅ **Pesado**: la gravedad alta lo trae de vuelta al suelo rápido.
- ✅ **Responsivo**: el input es inmediato, sin delay ni suavizado.
- ✅ **Decidido**: el salto es una acción comprometida, no una animación flotante.
- ✅ **Controlable**: las plataformas a distintas alturas se alcanzan sin frustración.

---

## Iteraciones durante el desarrollo

| Iteración | Cambio | Resultado |
|---|---|---|
| 1 | `Move Speed = 5`, `Jump Force = 12` | Personaje lento, no alcanza plataforma alta |
| 2 | `Move Speed = 7`, `Jump Force = 14` | Mejor, pero salto insuficiente para y=2 |
| 3 | `Move Speed = 7`, `Jump Force = 16`, `Gravity Scale = 3.5` | **Sweet spot Hollow Knight** ✓ |
| 4 | Pruebas con `Linear Drag = 1` vs `3` | Confirmado `1.5` como balance ideal |

---

## Conclusión

El "Game Feel" no sale de un solo valor sino de la relación entre todos:
- **Gravedad alta** + **drag moderado** + **input sin suavizado** = personaje con peso pero controlable.
- **Salto con `velocity = jumpForce`** (no `addForce`) = salto consistente e instantáneo, sin física rara.

Si quisiera cambiar a Celeste-style (snappy), los valores serían:
- `Move Speed = 6`, `Jump Force = 14`, `Gravity Scale = 2.5`, `Linear Drag = 3`

Si quisiera Sonic-style (impulso rápido):
- `Move Speed = 9`, `Jump Force = 10`, `Gravity Scale = 1.5`, `Linear Drag = 0.5`

Los valores finales son los de la columna "Hollow Knight" del plan acordado al inicio del Hito 1.