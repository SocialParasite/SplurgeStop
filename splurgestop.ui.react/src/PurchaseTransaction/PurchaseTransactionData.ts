import { http } from './../http';

export interface PurchaseTransactionData {
  purchaseTransactionId: string;
  storeName: string;
  purchaseDate: Date;
  totalPrice: number;
  itemCount: number;
}

export interface PurchaseTransactionDataFromServer {
  purchaseTransactionId: string;
  storeName: string;
  purchaseDate: Date;
  totalPrice: number;
  itemCount: number;
}

export const mapPurchaseTransactionFromServer = (
  transaction: PurchaseTransactionDataFromServer,
): PurchaseTransactionData => ({
  ...transaction,
  purchaseTransactionId: transaction.purchaseTransactionId,
  storeName: transaction.storeName,
  purchaseDate: new Date(transaction.purchaseDate),
  totalPrice: transaction.totalPrice,
  itemCount: transaction.itemCount,
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
