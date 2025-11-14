# HackyFox Mobile 
Juego educativo interactivo para niños, protagonizado por una mascota virtual. Diseñado en Unity para plataformas móviles, con enfoque en aprendizaje en ciberseguridad. Base de datos MySQL en workbench.
## Cómo abrir el proyecto
1. Clona el repositorio:
   ``bash
   git clone https://github.com/ThonyOziel/HackyFox_Mobile.git``

2. Abre Unity Hub → "Add project" → selecciona la carpeta clonada.
¡Listo! Ejecuta la escena principal desde Assets/Scenes/.

## Estructura

- `Assets/` → Scripts, escenas, prefabs, sprites
- `Packages/` → Dependencias de Unity
- `ProjectSettings/` → Configuración del proyecto
- `.gitignore` → Evita subir archivos pesados como `Library/`, `Build/`, etc.

## Contribuciones

1. Crea una rama descriptiva:
   ``bash
   git branch feature/nombre
   git push origin feature/nombre ``
2. Realiza tus cambios en el proyecto.
3. Verifica que los archivos ignorados no estén siendo rastreados
``bash
git status
git check-ignore -v Library/
git check-ignore -v Build/
git check-ignore -v obj/``
4. Haz commit y push de tu rama.
5. Abre un Pull Request hacia main o dev, según el flujo de trabajo acordado

 Cualquier duda comuniquen al grupo de whatsApp.
