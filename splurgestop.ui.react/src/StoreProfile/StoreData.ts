import { http } from './../http';

export interface StoreData {
  id: string;
  name: string;
}

export interface StoreDataFromServer {
  id: string;
  name: string;
}

export interface DetailedStoreData {
  id: { [key: string]: any[] };
  name: string;
}

export interface DetailedStoreDataFromServer {
  id: { [key: string]: any[] };
  name: string;
}

export const mapStoreFromServer = (store: StoreDataFromServer): StoreData => ({
  ...store,
  id: store.id,
  name: store.name,
});

export const mapDetailedStoreFromServer = (
  store: DetailedStoreDataFromServer,
): DetailedStoreData => ({
  ...store,
  id: store.id,
  name: store.name,
});

export const getStores = async (): Promise<StoreData[]> => {
  try {
    const result = await http<undefined, StoreDataFromServer[]>({
      path: '/Store',
    });
    if (result.parsedBody) {
      return result.parsedBody.map(mapStoreFromServer);
    } else {
      return [];
    }
  } catch (ex) {
    return [];
  }
};

export const getStore = async (
  id: string,
): Promise<DetailedStoreData | null> => {
  try {
    const result = await http<undefined, DetailedStoreDataFromServer>({
      path: `/Store/${id}`,
    });
    if (result.ok && result.parsedBody) {
      return mapDetailedStoreFromServer(result.parsedBody);
    } else {
      return null;
    }
  } catch (ex) {
    console.error(ex);
    return null;
  }
};
