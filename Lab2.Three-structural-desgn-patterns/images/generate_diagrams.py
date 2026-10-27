"""Rebuild the lab's SVG UML diagrams with Python 3; no dependencies needed."""
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

    def box(self, x, y, width, name, fields=(), methods=(), role=None):
        header = 60 if role else 40
        field_height = max(18, len(fields) * 23 + 12)
        height = header + field_height + len(methods) * 23 + 15
        self.parts.append(f'<rect x="{x}" y="{y}" width="{width}" height="{height}" fill="white" stroke="#334155"/>')
        self.parts.append(f'<rect x="{x}" y="{y}" width="{width}" height="{header}" fill="#dcfce7" stroke="#334155"/>')
        if role:
            self.text(x + 12, y + 21, f'«{role}»')
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


d = Diagram('Adapter | Make the legacy wand usable as a weapon', 1160, 700)
d.box(30, 110, 300, 'BattleArena', methods=['+ Fight(weapon): BattleResult'], role='Client')
d.box(710, 110, 400, 'IWeapon', ['+ Name: string {get}', '+ Damage: int {get}'], role='interface / Target')
d.box(710, 380, 400, 'WandAdapter', ['- _wand: LegacyWand', '+ Name: string {get}', '+ Damage: int {get}'], ['+ WandAdapter(wand)'], 'Adapter')
d.box(30, 380, 390, 'LegacyWand', ['+ Model: string {get}', '- _powerLevel: int'], ['+ LegacyWand(model, powerLevel)', '+ CalculateMagicPower(): int'], 'Adaptee')
d.edge('M330,170 H710')
d.text(435, 155, 'uses target interface', 'label')
d.edge('M910,380 V243', 'realization')
d.text(930, 320, 'implements', 'label')
d.edge('M710,475 H420', 'association')
d.text(475, 458, 'holds 1 / delegates', 'label')
d.text(30, 638, 'Name maps to Model; Damage maps to CalculateMagicPower(). LegacyWand is unchanged.', 'label')
d.text(30, 669, 'Dashed triangle: realization. Solid arrow: association. Dashed arrow: dependency.', 'label')
d.save('adapter.svg')

d = Diagram('Decorator | Stack enchantments through the same interface', 1200, 850)
d.box(420, 95, 360, 'IWeapon', ['+ Name: string {get}', '+ Damage: int {get}'], role='interface / Component')
d.box(30, 355, 380, 'Sword', ['+ Name: string {get}', '+ Damage: int {get}'], role='Concrete component')
d.box(700, 355, 450, 'WeaponDecorator', ['# Inner: IWeapon {get}', '+ Name: string {abstract, get}', '+ Damage: int {abstract, get}'], ['# WeaponDecorator(inner)'], 'abstract / Decorator')
d.box(30, 650, 440, 'FireEnchantment', ['+ Name: string {override, get}', '+ Damage: int {override, get}'], ['+ FireEnchantment(inner)'])
d.box(700, 650, 450, 'PoisonEnchantment', ['+ Name: string {override, get}', '+ Damage: int {override, get}'], ['+ PoisonEnchantment(inner)'])
d.edge('M220,355 V285 H490 V228', 'realization')
d.edge('M850,355 V285 H710 V228', 'realization')
d.edge('M1050,355 V175 H780', 'association')
d.text(885, 158, 'wraps exactly 1', 'label')
d.edge('M250,650 V600 H820 V534', 'inheritance')
d.edge('M925,650 V534', 'inheritance')
d.text(30, 820, 'Hollow triangles: realization (dashed) / inheritance (solid). # protected; + public.', 'label')
d.save('decorator.svg')

d = Diagram('Facade | One quest operation coordinates three subsystems', 1280, 800)
d.box(30, 110, 300, 'Program', methods=['+ Main(args): int'], role='Client')
d.box(620, 95, 600, 'ExpeditionFacade', ['- _equipment: EquipmentRack', '- _arena: BattleArena', '- _wallet: GoldWallet'], ['+ ExpeditionFacade(equipment, arena, wallet)', '+ CompleteQuest(weapon): QuestResult'], 'Facade')
d.box(30, 490, 370, 'EquipmentRack', ['+ EquippedWeapon: IWeapon? {get}'], ['+ Equip(weapon): void'], 'Subsystem')
d.box(455, 490, 370, 'BattleArena', methods=['+ Fight(weapon): BattleResult'], role='Subsystem')
d.box(880, 490, 340, 'GoldWallet', ['+ Balance: int {get}'], ['+ Deposit(amount): void'], 'Subsystem')
d.edge('M330,180 H620')
d.text(370, 162, 'CompleteQuest()', 'label')
d.edge('M720,297 V400 H215 V490', 'association')
d.edge('M920,297 V430 H640 V490', 'association')
d.edge('M1100,297 V490', 'association')
d.text(245, 388, '1. Equip', 'label')
d.text(665, 418, '2. Fight', 'label')
d.text(1115, 405, '3. Reward', 'label')
d.text(30, 711, 'The facade holds one instance of each subsystem. Rewards are deposited only after victory.', 'label')
d.text(30, 742, 'BattleResult and QuestResult are immutable result records; supporting signatures are simplified.', 'label')
d.text(30, 773, 'Solid arrow: association. Dashed arrow: dependency. + public; - private.', 'label')
d.save('facade.svg')
