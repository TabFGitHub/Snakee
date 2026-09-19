# Snakee — Unity

Классическая аркадная змейка на Unity. Пиксель-арт, звук, меню, пауза, рекорды.

## Геймплей
- Управление: WASD / стрелки
- Пауза: Esc
- Рестарт: R
- Цель: набрать максимальный счёт

## Особенности
- Плавная интерполяция сегментов
- Input buffering (нажатия обрабатываются заранее)
- Прогрессирующая скорость
- Локальный рекорд (PlayerPrefs)
- Полноценное меню + пауза + Game Over
- Input System (New)
- Пиксель-арт 16×16

## Технологии
- Unity 2D
- C#
- Unity Input System
- TextMeshPro

## 🎮 Играть

### Скачать готовый билд
- **Windows:** [Скачать v1.0.0](https://github.com/USERNAME/snake-unity/releases/latest)
- **WebGL (в браузере):** [Играть на itch.io](https://username.itch.io/snake-unity)

> 💡 Для WebGL ничего скачивать не нужно — игра запускается прямо в браузере.

### Запуск из исходников
1. Клонируй репозиторий:
   ```bash
   git clone https://github.com/USERNAME/snake-unity.git

2. Открой проект в Unity Hub (версия 2022.3 LTS).

3. Открой сцену Assets/Scenes/MainMenu.unity.

4. Нажми ▶ Play.

## Скриншоты
![Menu](docs/screenshots/menu.png)
![Gameplay](docs/screenshots/gameplay.png)
![Game Over](docs/screenshots/gameover.png)

## Документация
- [Game Design Document](GDD.md)

## Лицензия
MIT
