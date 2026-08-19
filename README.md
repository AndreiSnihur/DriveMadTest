# Drive Mad — прототип первого уровня

Клон первого уровня Drive Mad с упором на ощущение физики машины. Unity 6000.3.22f1 (URP), Input System, uGUI + TextMeshPro.

## Играть онлайн
WebGL-билд: **https://andreisnihur.github.io/DriveMadTest/** (ветка `gh-pages`, GitHub Pages).

## Запуск
1. Открыть корень проекта в Unity 6000.3.x.
2. Открыть `Assets/Scenes/Level_1.unity`, нажать Play. Для портретной раскладки поставить Game view в 9:16.

## Сборка
- Ориентация зафиксирована в Portrait (HUD и камера настроены под 9:16); дефолтный размер WebGL-канваса 540×960.
- WebGL: Gzip + Decompression Fallback — билд грузится с любого статического хостинга (GitHub Pages, itch.io)
  без настройки заголовков `Content-Encoding`; шаблон Minimal, имена файлов — хэши, Managed Stripping Medium.
- Android: ARM64, Managed Stripping Medium.
- Рендер: URP `Mobile_RPAsset` для WebGL/мобилок, `PC_RPAsset` для десктопа; тени, HDR и постпроцессинг выключены.

## Управление
- Клавиатура: `D` / `→` — вперёд, `A` / `←` — назад, `R` / `Space` — рестарт
- Тач: экранные кнопки `<` `>` и `RESTART` (эмулируют те же клавиши через Input System)

## Игровой цикл
Победа — доехать до финишных ворот, через 3 секунды уровень начинается заново. Поражение — приземлиться на крышу
или упасть с трассы: колёса отваливаются, через секунду уровень перезапускается. `R` перезапускает в любой момент.

## Структура
- `Assets/CodeBase/Car` — `CarMover` (мотор), `Wheel` (мотор хинджа, подвеска, отстрел), `CarDetacher` (краш),
  `RoofSensor`, `LooseWheel` (обломок колеса), `CarSettings` (весь тюнинг, ScriptableObject)
- `Assets/CodeBase/Game` — `LevelController` (победа / поражение / рестарт), `GameInput` (обёртка Input System)
- `Assets/CodeBase/Level` — `CarTriggerZone` (финиш / kill-зона)
- `Assets/CodeBase/Cameras` — `CameraFollower`
- `Assets/CodeBase/UI` — `HudView` (статус, тач-кнопки)
- `Assets/CodeBase/Input` — `CarControls.cs`, генерируется из `Assets/Input/CarControls.inputactions`
- `Assets/Prefabs` — `Car` (кузов + `Suspension` ×2 + `WheelPair` ×2), `LooseWheel`, `Level_1`
  (`Track_chunk_01` ×2, `Slope` из `Ramp` ×9, ворота `Start_Finish` ×2, триггеры финиша / kill-зоны),
  `System` (камера, свет, ввод, HUD)
- `Assets/Settings/CarSettings.asset` — настройки машины, `Assets/PhysicsMaterials` — `Wheel`, `Chassis`

## Физическая модель
2D-в-3D: машина едет вдоль **−Z**; у всех Rigidbody машины заморожены позиция по X и вращение по Y/Z.
Кузов + два хаба подвески (`ConfigurableJoint`, ход только по Y с пружинным драйвом) + два сферических колеса
на моторизованных `HingeJoint` (ось X). Реакция момента колёс естественно наклоняет кузов (вилли, стоппи, перевороты).
Коллайдер кузова — два бокса (корпус + кабина); триггер на крыше (`RoofSensor`) фиксирует приземление на крышу.
Физика считается с шагом 1/60 с (Project Settings → Time → Fixed Timestep).

Слои: `Ground` 6 (трасса), `Car` 7 (все части машины), `Debris` 8 (отстреленные колёса; `LooseWheel` лежит на этом
слое, а в момент отстрела ненадолго берёт слой машины, чтобы не выстрелить из-под кузова), `Zone` 9 (триггеры финиша
и kill-зоны). Матрица коллизий — только нужные пары: Car×Ground, Car×Debris, Car×Zone, Debris×Ground, Debris×Debris,
плюс Default×{Default, Ground, Car, Debris} как страховка для новых объектов; Car×Car, Zone×всё-кроме-Car, служебные
слои (UI, Water, TransparentFX, Ignore Raycast) и пустые слои — выключены.

## Тюнинг
Все параметры лежат в `Assets/Settings/CarSettings.asset` и применяются на лету в Play Mode: гравитация (30, мир
увеличен относительно реального масштаба; то же значение стоит в Project Settings → Physics), момент мотора,
максимальная скорость колёс, масса / центр масс / угловое затухание кузова, масса и инерция вращения колёс,
пружина и демпфер подвески. Значения в префабах синхронизированы с ассетом, но в рантайме источник правды — ассет.
Задержки рестарта после победы / поражения — на `LevelController` в префабе `System`.
