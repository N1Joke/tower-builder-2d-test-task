Версия Unity

Вопросы и уточнения по техническому заданию

-Установка кубиков в башню

ТЗ: "Остальные кубики ставятся в башню только если их перенесли поверх уже выставленных, в этом случае они анимацией подпрыгивают и ставятся сверху башни."
Помимо проверки по высоте, добавлена логика валидации по выступу: новый кубик должен находиться не менее чем на 50% площади над предыдущим, иначе установка не происходит.

-Удаление кубика через дыру

ТЗ: "В любой момент кубик из башни можно выкинуть в дыру, перенеся его на ее изображение в левой верхней части экрана. Необходимо учесть ее овальные границы при оценке попадания."
При попадании в область дыры куб удаляется, а кубики выше — плавно опускаются вниз.
В случае неудачного попадания, кубик совершает анимацию отскока обратно в башню — так как поведение в ТЗ не было описано.

-Информационные сообщения

ТЗ: "Каждое действие - установка кубика, выкидывание кубика, пропадание кубика, ограничение по высоте - должно сопровождаться надписью над нижней частью экрана."

Реализовано с поддержкой локализации.
(В полноценном проекте вывод сообщений стоило бы выделить в отдельный модуль, но в рамках задания это реализовано напрямую.)

-Подготовка к масштабированию

ТЗ: "Код должен предусматривать масштабирование для будущих обновлений (например, переносить кубик только на кубик такого же цвета в башне)."

Добавлен метод сравнения по id — при необходимости можно будет проверять соответствие любых атрибутов (цвет, тип и т.д.).

-Источники конфигурации

ТЗ: "Нужно учесть, что источником конфигурации могут стать разные источники данных (в игре может быть 1 реализация — из ScriptableObject)."
Можно создать bootstrap сцену с загрузчиком данных через JSON, с последующим обновлением ScriptableObject после десериализации. Таким образом, структура готова к расширяемости.

-Адаптация под разные экраны

ТЗ: "Необходимо чтобы мини-игра выглядела хорошо на основных соотношениях сторон: 19.5:9, 16:9, 4:3."
Использован параметр Size на камере — игра корректно отображается на указанных разрешениях.
Для масштабирования под большее количество устройств в будущем стоит добавить адаптивный Scaler, который будет подгонять фон и границы под референсное соотношение (например, 16:9).

Возможные доработки
-Архитектура
В рамках задания архитектурные требования не указывались.
Архитектура проекта основана на паттерне Context-Based Composition, где логика реализована в Disposable-классах, получающих зависимости через Ctx-структуры.
View-классы на сцене выступают как пассивные хранилища ссылок, без логики, что упрощает тестирование и повторное использование компонентов.
В качестве DI-контейнера используется Zenject, а точкой входа и сборки является MonoInstaller. Такой подход позволяет легко масштабировать проект и поддерживать чистое разделение ответственностей.

-Построение в край башни
На данный момент куб может быть установлен по краям.
В этом случае, если первый куб окажется на краю, башня может смещаться всё дальше в одну сторону и выйти за границы области строительства.
Это стоит доработать — например, ограничить зону постановки первого кубика или ввести дополнительную проверку на допустимые границы случайного смещения.


