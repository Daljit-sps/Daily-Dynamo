import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';

class LocalStorage implements Storage {
  [name: string]: any;
  length!: number;
  clear(): void {}
  getItem(key: string): string | null {
    return null;
  }
  key(index: number): string | null {
    return null;
  }
  removeItem(key: string): void {}
  setItem(key: string, value: string): void {}
}

@Injectable({ providedIn: 'root' })
export class LocalStorageService implements Storage {
  storage: Storage = new LocalStorage();
  constructor(@Inject(PLATFORM_ID) private platformId: string) {
    if (isPlatformBrowser(platformId)) this.storage = localStorage;
    else throw new Error('LocalStorage is not available on this platform');

    if (!this.storage) return;
    this.length = localStorage.length;
  }

  [name: string]: any;
  length: number = 0;

  clear = (): void => this.storage.clear();

  getItem = (key: string): any | null => {
    const value = this.storage.getItem(key);
    if (!value) return null;
    return JSON.parse(value);
  };

  key = (index: number): string | null => this.storage.key(index);

  removeItem = (key: string): void => {
    this.length -= 1;
    return this.storage.removeItem(key);
  };

  setItem = (key: string, value: unknown): void => {
    this.length += 1;
    return this.storage.setItem(key, JSON.stringify(value));
  };
}
