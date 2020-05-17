import {
  Action,
  ActionCreator,
  Dispatch,
  Reducer,
  combineReducers,
  Store,
  createStore,
  applyMiddleware,
} from 'redux';
import thunk, { ThunkAction } from 'redux-thunk';
import {
  PurchaseTransactionData,
  getPurchaseTransactions,
} from './PurchaseTransaction/PurchaseTransactionData';

interface PurchaseTransactionState {
  readonly loading: boolean;
  readonly transactions: PurchaseTransactionData[] | null;
}

interface GettingPurchaseTransactionsAction
  extends Action<'GettingPurchaseTransactions'> {}

export interface AppState {
  readonly transactions: PurchaseTransactionState;
}

export interface GotPurchaseTransactionsAction
  extends Action<'GotPurchaseTransactions'> {
  transactions: PurchaseTransactionData[];
}

type PurchaseTransactionActions =
  | GettingPurchaseTransactionsAction
  | GotPurchaseTransactionsAction;

const initialPurchaseTransactionState: PurchaseTransactionState = {
  loading: false,
  transactions: null,
};

export const getPurchaseTransactionsActionCreator: ActionCreator<ThunkAction<
  Promise<void>,
  PurchaseTransactionData[],
  null,
  GotPurchaseTransactionsAction
>> = () => {
  return async (dispatch: Dispatch) => {
    const gettingPurchaseTransactionsAction: GettingPurchaseTransactionsAction = {
      type: 'GettingPurchaseTransactions',
    };
    dispatch(gettingPurchaseTransactionsAction);

    const transactions = await getPurchaseTransactions();

    const gotPurchaseTransactionAction: GotPurchaseTransactionsAction = {
      transactions,
      type: 'GotPurchaseTransactions',
    };
    dispatch(gotPurchaseTransactionAction);
  };
};

const purchaseTransactionsReducer: Reducer<
  PurchaseTransactionState,
  PurchaseTransactionActions
> = (state = initialPurchaseTransactionState, action) => {
  switch (action.type) {
    case 'GettingPurchaseTransactions': {
      return {
        ...state,
        transactions: null,
        loading: true,
      };
    }
    case 'GotPurchaseTransactions': {
      return {
        ...state,
        transactions: action.transactions,
        loading: false,
      };
    }
    default:
      neverReached(action);
  }
  return state;
};

const neverReached = (never: never) => {};

const rootReducer = combineReducers<AppState>({
  transactions: purchaseTransactionsReducer,
});

export function configureStore(): Store<AppState> {
  const store = createStore(rootReducer, undefined, applyMiddleware(thunk));
  return store;
}
