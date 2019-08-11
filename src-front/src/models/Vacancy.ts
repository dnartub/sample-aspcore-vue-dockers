export class Vacancy {
    // для инициализации всех свойств класса во избежания проблем с реактивностью
    public constructor(init?: Partial<Vacancy>) {
        Object.assign(this, init);
    }
    static fromArray(array: Array<any>): Vacancy[] {
        let sources: Vacancy[] = [];
        array.forEach(x => sources.push(new Vacancy(x)));
        return sources;
    }

    name: string;
    salary: string;
    address: string;
    company: string;
    annotation: string;
    url: string;
}