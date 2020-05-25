import { http } from './../http';

export interface PurchaseTransactionData {
  id: string;
  storeName: string;
  purchaseDate: Date;
  totalPrice: string;
  itemCount: number;
}

export interface PurchaseTransactionDataFromServer {
  id: string;
  storeName: string;
  purchaseDate: Date;
  totalPrice: string;
  itemCount: number;
}

export interface DetailedPurchaseTransactionData {
  id: { [key: string]: any[] };
  purchaseDate: { [key: string]: any[] };
  store: { [key: string]: any[] };
  lineItems: [
    {
      id: string;
      price: {
        booking: number;
        amount: number;
        currency: {
          currencyCode: string;
          currencySymbol: string;
          positionRelativeToSource: string;
        };
      };
    },
  ];
  notes: string;
  totalPrice: string;
}

export interface DetailedPurchaseTransactionDataFromServer {
  id: { [key: string]: any[] };
  purchaseDate: { [key: string]: any[] };
  store: { [key: string]: any[] };
  lineItems: [
    {
      id: string;
      price: {
        booking: number;
        amount: number;
        currency: {
          currencyCode: string;
          currencySymbol: string;
          positionRelativeToSource: string;
        };
      };
    },
  ];
  notes: string;
  totalPrice: string;
}

export const mapPurchaseTransactionFromServer = (
  transaction: PurchaseTransactionDataFromServer,
): PurchaseTransactionData => ({
  ...transaction,
  id: transaction.id,
  storeName: transaction.storeName,
  purchaseDate: new Date(transaction.purchaseDate),
  totalPrice: transaction.totalPrice,
  itemCount: transaction.itemCount,
});

export const mapDetailedPurchaseTransactionFromServer = (
  transaction: DetailedPurchaseTransactionDataFromServer,
): DetailedPurchaseTransactionData => ({
  ...transaction,
  id: transaction.id,
  purchaseDate: transaction.purchaseDate,
  store: transaction.store,
  lineItems: transaction.lineItems,
  totalPrice: transaction.totalPrice,
  notes: transaction.notes,
});

export const getPurchaseTransactions = async (): Promise<
  PurchaseTransactionData[]
> => {
  try {
    const result = await http<undefined, PurchaseTransactionDataFromServer[]>({
      path: '/PurchaseTransaction',
    });
    if (result.parsedBody) {
      return result.parsedBody.map(mapPurchaseTransactionFromServer);
    } else {
      return [];
    }
  } catch (ex) {
    return [];
  }
};

export const getPurchaseTransaction = async (
  id: string,
): Promise<DetailedPurchaseTransactionData | null> => {
  try {
    const result = await http<
      undefined,
      DetailedPurchaseTransactionDataFromServer
    >({
      path: `/PurchaseTransaction/${id}`,
    });
    if (result.ok && result.parsedBody) {
      return mapDetailedPurchaseTransactionFromServer(result.parsedBody);
    } else {
      return null;
    }
  } catch (ex) {
    console.error(ex);
    return null;
  }
};
