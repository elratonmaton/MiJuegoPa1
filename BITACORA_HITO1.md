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

---

## Bugs y correcciones durante el desarrollo

Documentación honesta de los problemas encontrados y cómo se resolvieron. **Cada bug es una lección aprendida.**

### Bug 1 — Input System vs Input Manager (legacy)

**Síntoma:** `InvalidOperationException: You are trying to read Input using the UnityEngine.Input class, but you have switched active Input handling to Input System package in Player Settings.`

**Causa:** El template URP 2D viene con `activeInputHandler: 1` (solo Input System package). Mi script usaba `Input.GetAxisRaw()` y `Input.GetButtonDown()` del Input Manager legacy.

**Fix:** Reescribir el script usando `Keyboard.current.leftArrowKey.isPressed` y `kb.spaceKey.wasPressedThisFrame` con `using UnityEngine.InputSystem`.

**Lección:** Verificar `ProjectSettings/ProjectSettings.asset → activeInputHandler` antes de escribir código de input.

---

### Bug 2 — Build Settings no incluía Laboratorio.unity

**Síntoma:** Al abrir el proyecto, Unity cargaba `SampleScene.unity` (vacía). El usuario veía solo lo que agregaba manualmente.

**Causa:** `EditorBuildSettings.asset` solo listaba `SampleScene.unity`. Al ser esa la primera escena de la lista, Unity la abría al iniciar.

**Fix:** Añadir `Assets/_Project/Scenes/Laboratorio.unity` como índice 0 (primera escena) en Build Settings y deshabilitar `SampleScene`.

**Lección:** Las escenas generadas por código deben añadirse a Build Settings para que Unity las cargue al abrir el proyecto.

---

### Bug 3 — SpriteRenderer con class ID incorrecto

**Síntoma:** `The referenced script on this Behaviour is missing!` en todos los GameObjects visuales. La escena aparecía vacía.

**Causa:** Escribí todos los `SpriteRenderer` con `--- !u!114` (class ID de MonoBehaviour) cuando el correcto es `--- !u!212` (SpriteRenderer). 15 componentes mal clasificados.

**Fix:** Cambiar `--- !u!114` → `--- !u!212` solo donde el siguiente bloque es `SpriteRenderer:` (regex para distinguirlo de MonoBehaviour real).

**Lección:** Los class IDs de Unity son específicos por componente. SpriteRenderer ≠ MonoBehaviour, aunque ambos serialicen igual.

---

### Bug 4 — Rigidbody2D Constraints = FreezePositionX (1)

**Síntoma:** El personaje flipX funcionaba al presionar flechas pero no se desplazaba horizontalmente. Los logs confirmaban que `Update()` se ejecutaba y Keyboard.current detectaba input.

**Causa:** `m_Constraints: 1` en `Rigidbody2D` significa `FreezePositionX` (congelar posición X), bloqueando el movimiento horizontal. Yo quería `FreezeRotation Z` que es el valor `4` en el enum.

**Enum correcto:**
```
RigidbodyConstraints2D.None = 0
FreezePositionX = 1     ← mi error
FreezePositionY = 2
FreezePosition = 3
FreezeRotation = 4      ← lo correcto
FreezeAll = 7
```

**Fix:** Cambiar `m_Constraints: 1` → `m_Constraints: 4` en el Rigidbody2D del Player.

**Lección:** Los valores numéricos de los enums son posicionales. Si dos valores tienen significados opuestos (mover vs congelar), intercambiarlos produce bugs silenciosos.

---

### Bug 5 — Tag "Player" duplicado en TagManager

**Síntoma:** Warning `Default GameObject Tag: Player already registered` en Console.

**Causa:** Añadí `Player` como tag custom en `TagManager.tags`, pero ya existe como tag built-in de Unity.

**Fix:** Quitar `Player` del array `tags` en TagManager (quedarse solo con `Ground`, `Obstacle`, `Coin`).

**Lección:** Los tags built-in (`Untagged`, `Respawn`, `Finish`, `EditorOnly`, `MainCamera`, `Player`, `GameController`) NO deben listarse en el array `tags`.

---

### Bug 6 — GameObject "Barril_1" con formato YAML corrupto

**Síntoma:** `Broken text PPtr in file. Local file identifier (101000) doesn't exist!` y `Found a Transform component that is not assigned to a GameObject`.

**Causa:** Al hacer `cat >> archivo` para añadir el Barril_1, el bloque se pegó a la línea anterior sin newline (`m_MaterialDirty: 0--- !u!1 &101000`). Unity no reconoció el GameObject padre y todos sus componentes hijos (Transform, SpriteRenderer, BoxCollider2D, Rigidbody2D) quedaron huérfanos.

**Fix:** Insertar `\n` antes de `--- !u!1 &101000`.

**Lección:** El formato de escenas de Unity es YAML con separadores `---` al inicio de línea. Cada componente DEBE empezar en línea nueva.

---

### Bug 7 — CircleCollider2D con class ID incorrecto (60 → 59 → 58)

**Síntoma:** Tres iteraciones de error:
1. `Type mismatch. Expected PolygonCollider2D, but found CircleCollider2D` (cuando usé `!u!60`)
2. `Type mismatch. Expected HingeJoint, but found CircleCollider2D` (cuando cambié a `!u!59`)
3. Trigger no se disparaba (cuando usé `!u!59` con serializedVersion faltante)

**Causa:** Class IDs de Unity 6 (verificados desde documentación oficial):
| Componente | Class ID |
|---|---|
| Rigidbody2D | `50` |
| CircleCollider2D | `58` ← CORRECTO |
| BoxCollider2D | `61` |
| EdgeCollider2D | `68` |
| PolygonCollider2D | `60` |
| HingeJoint (3D) | `59` |
| HingeJoint2D | `233` |
| SpriteRenderer | `212` |

**Fix:** Cambiar todos los `--- !u!59` → `--- !u!58` para CircleCollider2D.

**Lección crítica:** Los class IDs de Unity son la fuente más común de errores en escenas generadas por código. **Verificar contra la documentación oficial de Unity antes de escribir cada class ID**, no asumir secuencialidad.

---

### Bug 8 — CircleCollider2D sin `serializedVersion: 2`

**Síntoma:** El script `Coin.cs` se ejecutaba (Awake aparecía en Console) pero `TryGetComponent<CircleCollider2D>()` retornaba `false`. El collider estaba "huérfano" silenciosamente.

**Causa:** Faltaba el campo `serializedVersion: 2` al inicio del bloque `CircleCollider2D`. Unity lo descartaba silenciosamente porque estaba "por debajo del mínimo soportado".

**Fix:** Añadir `serializedVersion: 2` después de `CircleCollider2D:` en cada uno.

**Lección:** Cada componente serializado en Unity debe declarar su `serializedVersion`. Sin él, Unity asume versión 1 y descarta el componente.

---

### Bug 9 — CollisionDetectionMode = Discrete causa tunneling

**Síntoma:** Las monedas no desaparecían al saltar sobre ellas (incluso con triggers correctos).

**Causa:** En el salto, el Player se mueve ~0.3 unidades por frame, pero el `CircleCollider2D` de la moneda tiene radio 0.16. Con `CollisionDetectionMode = Discrete`, Unity solo detecta intersecciones al final del frame → el Player atraviesa la moneda sin disparar el trigger (tunneling).

**Fix:** Cambiar el `m_CollisionDetectionMode` del Player de `0` (Discrete) a `1` (Continuous). Esto hace raycast continuo y detecta todas las colisiones del frame.

**Lección:** A velocidades altas (saltos, dashes), usar `Continuous` collision detection para evitar tunneling en colliders pequeños.

---

## Reflexión final

Cada uno de estos 9 bugs fue una oportunidad de aprendizaje. La mayoría vinieron por:

1. **Asumir formato YAML de Unity** sin haberlo verificado antes.
2. **No verificar class IDs** contra la documentación oficial.
3. **Construir la escena por código** sin pasar por el editor (más rápido pero propenso a errores estructurales).

**Lección meta:** Para futuros hitos, generar las escenas y assets visuales dentro de Unity Editor (con las herramientas GUI), reservando el código para scripts y configuración.**

---

## Historial de commits del Hito 1

```
2760c9f fix: CircleCollider2D class ID correcto es 58 (no 59 ni 60)
5ec8ed4 fix: añadir serializedVersion: 2 a CircleCollider2D
f01909e debug: añadir Awake log + OnTriggerStay2D en Coin
e63a2f7 fix: CollisionDetectionMode del Player a Continuous
8bcb30f docs: añadir bitácora digital del Hito 1
ee71b5c fix: 3 errores estructurales en Laboratorio.unity
e941ac2 fix: Rigidbody2D Constraints era FreezePositionX (1) en vez de FreezeRotation (4)
2ac7bff debug: añadir logs en PlayerController para diagnosticar input
0093d66 fix: SpriteRenderer debe ser class ID 212, no 114
1334070 fix: migrar PlayerController a Input System
1e84d06 fix: añadir Laboratorio.unity a Build Settings
ea993a2 docs: añadir README del Hito 1
73d5600 M3: PlayerController con Game Feel (Hollow Knight-style)
aa9d1b2 M2: Añadidos elementos interactivos
a809a6d M1: Estructura _Project/ y construcción del escenario
0fc4e7d Initial check-in
```