import { Injectable } from '@angular/core';
import { NgbDateStruct, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';


function padNumber(value: number) {
  if (isNumber(value)) {
      return `0${value}`.slice(-2);
  } else {
      return '';
  }
}

function isNumber(value: any): boolean {
  return !isNaN(toInteger(value));
}

function toInteger(value: any): number {
  return parseInt(`${value}`, 10);
}

@Injectable()
export class CustomDateParserFormatter extends NgbDateParserFormatter{
  parse(value: string): NgbDateStruct
  {
    // if (!value) {
    //   return null;
    // }
    // const parts = value.split('/');
    // return { day: + parts[2], month: + parts[1], year: + parts[0]} as NgbDateStruct;

    if (value) {
      const dateParts = value.trim().split('/');
      if (dateParts.length === 1 && isNumber(dateParts[0])) {
          return {year: toInteger(dateParts[0]), month: null, day: null};
      } else if (dateParts.length === 2 && isNumber(dateParts[0])
          && isNumber(dateParts[1])) {
          return {year: toInteger(dateParts[1]), month: toInteger(dateParts[0]), day: null};
      } else if (dateParts.length === 3 && isNumber(dateParts[0])
          && isNumber(dateParts[1]) && isNumber(dateParts[2])) {
          return {year: toInteger(dateParts[2]), month: toInteger(dateParts[1]), day: toInteger(dateParts[0])};
      }
  }
    return null;

  }
  format(date: NgbDateStruct): string
  {
    //  return date ? ('0' + date.day).slice(-2) + '/' + ('0' + date.month).slice(-2) + '/' + date.year : null;

    let stringDate = '';
    if (date) {
            stringDate += isNumber(date.day) ? padNumber(date.day) + '/' : '';
            stringDate += isNumber(date.month) ? padNumber(date.month) + '/' : '';
            stringDate += date.year;
        }
    return stringDate;
  }
}
