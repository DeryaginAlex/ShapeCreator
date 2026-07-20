# Shape Creator

<img width="800" alt="image" src="https://github.com/user-attachments/assets/e8daa222-1d42-4871-a751-652376c6de89" />

### Стек Технологий

* Visual Studio 2022

* .Net 9

* WPF

* MVVM

* Nuget: CommunityToolkit.Mvvm, Nunit, Moq

### Архитектура

* `ConfigService` для запуска тестового проекта

* `FileService` для работы с файлами проектов (сохранение/загрузка проектов, работа с файлами логов)

* `LoggerService` для сохранения информации об ошибках

* `UiService` для отрисовки UI элементов

* `ValidateService` для валидации моделей

### Добавление новой фигуры

Заполните поля `Name`, `ShapeType`, `Start X`, `Start Y`, `End X`, `End Y`, `Actice` в пустой строке DataGride

Нажмите клавишу `Enter`

### Изменение существующей фигуры

Измените значения в полях `Name`, `ShapeType`, `Start X`, `Start Y`, `End X`, `End Y`, `Actice`

Нажмите клавишу `Enter`

### Удаление существующей фигуры

Выберите строку

Нажмите клавишу  `Delete`

### Валидация полей

Валидация полей происходит только при попытке сохранения проекта

<img width="800" alt="image" src="https://github.com/user-attachments/assets/f6014b3a-539d-46ce-8efc-5d547104e27f" />

### JSON Schema

```json
{
  "shapes": [
    {
      "id": string,
      "name": string,
      "shapeType": string,
      "coordinateStart": {
        "x": integer,
        "y": integer
      },
      "coordinateFinish": {
        "x": integer,
        "y": integer
      },
      "isActive": boolean
    }
  ]
}
```

, где

`shapes` - массив фигур

`id` - уникальный идентификатор фигуры (Guid)

`name` - имя фигуры

`shapeType` - тип фигуры (Circle/Square/Rhombus)

Каждую из этих фугур можно вписать в пямоугольник
Размер и местоположение фигур определяется за счет координат этого прямоугольника

`coordinateStart` - левая нижняя точка прямоугольника в который будет вписана фигура

`coordinateFinish` - правая верхняя точка прямоугольника в который будет вписана фигура

`X` - позиция на оси абсцисс

`Y` - позиция на оси ординат 

`isActive` - будет ли показана фигура на графике

### NUnit

<img width="400" alt="image" src="https://github.com/user-attachments/assets/3230792e-618b-41fd-a939-d5fb32a9aaf2" />
