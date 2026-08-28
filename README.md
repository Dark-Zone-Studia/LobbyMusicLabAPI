# 🎵 LobbyMusicLabAPI

**LobbyMusicLabAPI** — это плагин для серверов SCP: Secret Laboratory, построенный на базе LabApi. Он автоматизирует проигрывание фоновой музыки в формате .ogg в лобби и после завершения раунда.

## ⚠️ Обязательная зависимость
Для работы этого плагина **необходимо** установить:
* AudioPlayerApi (от Killers0992): [Скачать здесь](https://plugins.scpslgame.com/plugin/Killers0992/AudioPlayerApi)
  * Без этого API функции воспроизведения звука работать не будут.

## 🛠 Установка
### 🪟 Windows
1.  Поместите LobbyMusicLabAPI.dll в LabAPI/Plugins.
2.  Запустите сервер для генерации папок.
3.  Добавьте .ogg файлы в:
  *  %AppData%/SCP Secret Laboratory/LabAPI/audio **(Лобби)**
     * %AppData%/SCP Secret Laboratory/LabAPI/audio_end **(Конец раунда)**

### 🐧 Linux (Ubuntu/Debian/CentOS)
1. Поместите LobbyMusicLabAPI.dll в директорию плагинов вашего сервера.
2. Пути для аудиофайлов обычно находятся здесь:
 * ~/.config/SCP Secret Laboratory/LabAPI/audio
   * ~/.config/SCP Secret Laboratory/LabAPI/audio_end
   * (Или в корневой папке сервера внутри .config)
3. Убедитесь, что у пользователя, запускающего сервер, есть права на чтение/запись в этих папках (chmod +x).

### ⚙️ Конфигурация (MusicPlugin.yml)
Плагин автоматически создает конфиг со следующими параметрами:
```yaml
# Включен ли плагин?
is_enabled: true
# Громкость музыки в ЛОББИ (0.0 - 1.0)
music_volume: 5
# Громкость музыки в КОНЦЕ раунда (0.0 - 1.0)
end_round_volume: 0.600000024
# Игнорируется в режиме рандома.
lobby_song_path: music.ogg
```
