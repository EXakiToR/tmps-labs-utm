"""Generate dependency-free SVG UML class diagrams. Run with Python 3."""
from html import escape
from pathlib import Path

OUT = Path(__file__).resolve().parent


class Diagram:
    def __init__(self, title, width, height):
        self.parts = [f'''<svg xmlns="http://www.w3.org/2000/svg" width="{width}" height="{height}" viewBox="0 0 {width} {height}">
<title>{escape(title)}</title>
<defs>
<marker id="triangle" markerWidth="16" markerHeight="16" refX="15" refY="8" orient="auto" markerUnits="userSpaceOnUse"><path d="M1,1 L15,8 L1,15 Z" fill="white" stroke="#334155"/></marker>
<marker id="arrow" markerWidth="12" markerHeight="12" refX="11" refY="6" orient="auto" markerUnits="userSpaceOnUse"><path d="M1,1 L11,6 L1,11" fill="none" stroke="#334155"/></marker>
</defs>
<rect width="100%" height="100%" fill="#f8fafc"/>
<style>text {{font-family:Consolas,monospace;font-size:14px;fill:#0f172a}} .title {{font-family:Segoe UI,sans-serif;font-size:24px;font-weight:700}} .label {{font-family:Segoe UI,sans-serif;font-size:14px;fill:#475569}}</style>''']
        self.text(28, 38, title, 'title')

    def text(self, x, y, value, css=''):
        self.parts.append(f'<text x="{x}" y="{y}" class="{css}">{escape(value)}</text>')

    def box(self, x, y, width, name, fields=(), methods=(), stereotype=None):
        header = 60 if stereotype else 40
        field_height = max(18, len(fields) * 23 + 12)
        height = header + field_height + len(methods) * 23 + 15
        self.parts.append(f'<rect x="{x}" y="{y}" width="{width}" height="{height}" fill="white" stroke="#334155"/>')
        self.parts.append(f'<rect x="{x}" y="{y}" width="{width}" height="{header}" fill="#dbeafe" stroke="#334155"/>')
        if stereotype:
            self.text(x + 12, y + 21, f'«{stereotype}»')
        self.text(x + 12, y + header - 13, name)
        for i, field in enumerate(fields):
            self.text(x + 12, y + header + 24 + i * 23, field)
        divider = y + header + field_height
        self.parts.append(f'<path d="M{x},{divider} H{x + width}" stroke="#334155"/>')
        for i, method in enumerate(methods):
            self.text(x + 12, divider + 24 + i * 23, method)

    def edge(self, path, kind='dependency'):
        dashed = ' stroke-dasharray="7 5"' if kind in ('dependency', 'realization') else ''
        marker = 'triangle' if kind in ('inheritance', 'realization') else 'arrow'
        self.parts.append(f'<path d="{path}" fill="none" stroke="#334155" stroke-width="1.5"{dashed} marker-end="url(#{marker})"/>')

    def save(self, name):
        self.parts.append('</svg>')
        (OUT / name).write_text('\n'.join(self.parts), encoding='utf-8')


d = Diagram('Singleton | One shared game journal', 1000, 540)
d.box(30, 110, 260, 'Program', methods=['+ Main(args): int'])
d.box(30, 300, 260, 'Encounter', methods=['+ Play(hero): BattleResult'], stereotype='abstract')
d.box(510, 110, 450, 'GameJournal',
      ['- _instance: Lazy<GameJournal> {static}', '- _entries: List<string>', '- _gate: object'],
      ['- GameJournal()', '+ Instance: GameJournal {static, get}', '+ Record(entry: string): void', '+ GetEntries(): IReadOnlyList<string>'], 'sealed / Singleton')
d.edge('M290,160 H510')
d.text(335, 148, 'reads Instance', 'label')
d.edge('M290,370 H400 V300 H510')
d.text(310, 355, 'records events', 'label')
d.text(30, 485, 'Dashed arrow: dependency. + public; - private; {static} class-level member.', 'label')
d.text(30, 512, 'Lazy<T> creates one instance; a lock protects journal writes and snapshots.', 'label')
d.save('singleton.svg')

d = Diagram('Builder | Construct immutable heroes step by step', 1200, 830)
d.box(30, 100, 430, 'HeroDirector', methods=['+ CreateKnight(builder): Hero {static}', '+ CreateRanger(builder): Hero {static}'])
d.box(680, 100, 470, 'IHeroBuilder', methods=['+ Named(name): IHeroBuilder', '+ WithHealth(health): IHeroBuilder', '+ WithAttack(attack): IHeroBuilder', '+ AddEquipment(item): IHeroBuilder', '+ Build(): Hero'], stereotype='interface')
d.box(680, 435, 470, 'HeroBuilder', ['- _name: string', '- _health: int = 100', '- _attack: int = 10', '- _equipment: List<string>'],
      ['+ Named(name): IHeroBuilder', '+ WithHealth(health): IHeroBuilder', '+ WithAttack(attack): IHeroBuilder', '+ AddEquipment(item): IHeroBuilder', '+ Build(): Hero'])
d.box(30, 470, 430, 'Hero', ['+ Name: string {get}', '+ Health: int {get}', '+ Attack: int {get}', '+ Equipment: IReadOnlyList<string> {get}'],
      ['~ Hero(name, health, attack, equipment)'])
d.edge('M460,155 H680')
d.text(515, 142, 'uses recipe', 'label')
d.edge('M915,435 V308', 'realization')
d.text(930, 390, 'implements', 'label')
d.edge('M680,575 H460')
d.text(515, 562, 'creates', 'label')
d.text(30, 778, 'Dashed hollow triangle: interface realization. Dashed open arrow: dependency.', 'label')
d.text(30, 805, '+ public; - private; ~ internal. Hero copies equipment into a read-only collection.', 'label')
d.save('builder.svg')

d = Diagram('Factory Method | Locations choose enemy products', 1260, 850)
d.box(30, 100, 490, 'Encounter', methods=['+ CreateEnemy(): IEnemy {abstract}', '+ Play(hero: Hero): BattleResult'], stereotype='abstract / Creator')
d.box(800, 100, 410, 'IEnemy', ['+ Name: string {get}', '+ Health: int {get}', '+ Attack: int {get}', '+ Reward: int {get}'], stereotype='interface / Product')
d.box(30, 435, 490, 'ForestEncounter', methods=['+ CreateEnemy(): IEnemy'], stereotype='Concrete creator')
d.box(30, 640, 490, 'MountainEncounter', methods=['+ CreateEnemy(): IEnemy'], stereotype='Concrete creator')
d.box(800, 435, 410, 'Goblin', ['+ Name = "Goblin"', '+ Health = 40; Attack = 8; Reward = 10'], stereotype='Concrete product')
d.box(800, 640, 410, 'Dragon', ['+ Name = "Dragon"', '+ Health = 90; Attack = 20; Reward = 50'], stereotype='Concrete product')
d.edge('M520,170 H800')
d.text(600, 156, 'uses / returns', 'label')
d.edge('M180,435 V239', 'inheritance')
d.edge('M30,685 H15 V310 H100 V239', 'inheritance')
d.edge('M940,435 V279', 'realization')
d.edge('M1210,685 H1240 V330 H1080 V279', 'realization')
d.edge('M520,495 H800')
d.text(610, 482, 'creates', 'label')
d.edge('M520,700 H800')
d.text(610, 687, 'creates', 'label')
d.text(30, 802, 'Solid hollow triangle: inheritance. Dashed hollow triangle: interface realization.', 'label')
d.text(30, 827, 'Dashed open arrow: dependency. Play invokes CreateEnemy through polymorphism.', 'label')
d.save('factory.svg')
