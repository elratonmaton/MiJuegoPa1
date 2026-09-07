# Mi Juego PA1 — Hito 1

Prototipo 2D de plataformas estilo Hollow Knight. Movimiento responsivo, físicas pesadas y сбор coleccionables interactivos. Proyecto académico de prueba técnica para estudio indie.

---

## 🎮 Controles

| Acción | Tecla |
|---|---|
| Mover | `←` / `→` o `A` / `D` |
| Saltar | `Space` |

---

## 🚀 Cómo abrir el proyecto

1. Abre Unity Hub → `Add` → selecciona esta carpeta.
2. Versión recomendada: **Unity 6 LTS** (URP 2D).
3. Abre la escena `Assets/_Project/Scenes/Laboratorio.unity`.
4. Pulsa **Play**.

---

## 📁 Estructura de carpetas

```
Assets/
├── _Project/
│   ├── Scenes/      → Laboratorio.unity
│   ├── Scripts/     → PlayerController.cs, Coin.cs
│   ├── Prefabs/     → (reservado)
│   └── Materials/   → (reservado)
├── Sprites/
│   └── Berie's_Adventure_Seaside_Asset_Pack_Free/
├── Scenes/          → SampleScene.unity (template)
├── Settings/        → URP 2D config
└── Welcome/         → Tutorial Unity
```

---

## 🎯 Game Feel (Hollow Knight-style)

| Parámetro | Valor | Por qué |
|---|---|---|
| Move Speed | `7` | Velocidad horizontal responsiva |
| Jump Force | `16` | Salto alto y decidido |
| Gravity Scale | `3.5` | Caída pesada y rápida |
| Mass | `1` | Estándar para personajes |
| Linear Drag | `1.5` | Frenado sutil sin inercia flotante |

Todo se ajusta desde el Inspector en el `Player` → componente `Player Controller`.

---

## 📦 Commits del Hito 1

| Hash | Mensaje | Alcance |
|---|---|---|
| `a809a6d` | `M1: Estructura _Project/ y construcción del escenario` | Carpetas + escena + escenario |
| `aa9d1b2` | `M2: Añadidos elementos interactivos` | `Coin.cs` + 2 barriles + 3 monedas + tags |
| `73d5600` | `M3: PlayerController con Game Feel` | `PlayerController.cs` + Player GameObject |

---

## 🛠️ Stack técnico

- **Engine:** Unity 6 LTS
- **Render Pipeline:** URP 2D
- **Lenguaje:** C# (`MonoBehaviour`, `Rigidbody2D`, `Collider2D`)
- **Versionado:** Git + GitHub
- **Assets:** [Berie's Adventure — Seaside Asset Pack (Free)](https://opengameart.org/)

---

## ⚠️ Notas de portabilidad

- Los `.meta` de los assets están commiteados; los GUIDs son estables.
- Si clonas el repo en otra máquina, Unity regenerará `Library/` y `Temp/` automáticamente.
- El `.gitignore` excluye `Library/`, `Temp/`, `Logs/`, `UserSettings/` y `.DS_Store`.