import { PlanBenefit } from "./plan-benefit-model";

export class Plan {
  id: number;
  description: string;
  createDate: Date;
  value: number;
  monthNumber: number;
  planBenefit: PlanBenefit[] = [];
  partnerTypeId: number;
  active: boolean;
  public constructor(init?: Partial<Plan>) {
    Object.assign(this, init);
  }

  static fromJson(jsonData: any): Plan {
    return Object.assign(new Plan(), jsonData);
  }
}
