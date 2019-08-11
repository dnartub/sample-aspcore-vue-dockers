export class Source {
    // ��� ������������� ���� ������� ������ �� ��������� ������� � �������������
    public constructor(init?: Partial<Source>) {
        Object.assign(this, init);
    }
    static fromArray(array: Array<any>): Source[] {
        let sources: Source[] = [];
        array.forEach(x => sources.push(new Source(x)));
        return sources;
    }

    id: string;
    url: string;
}