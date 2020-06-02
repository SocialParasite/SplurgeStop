import { Action, ActionCreator, Dispatch, Reducer } from 'redux';
import { ThunkAction } from 'redux-thunk';
import { StoreData, getStores } from './StoreData';

export interface StoreState {
  readonly loading: boolean;
  readonly stores: StoreData[] | null;
}

interface GettingStoresAction extends Action<'GettingStores'> {}

interface AppState {
  readonly stores: StoreState;
}

export interface GotStoresAction extends Action<'GotStores'> {
  stores: StoreData[];
}

type StoreActions = GettingStoresAction | GotStoresAction;

export const initialStoreState: StoreState = {
  loading: false,
  stores: null,
};

export const getStoresActionCreator: ActionCreator<ThunkAction<
  Promise<void>,
  StoreData[],
  null,
  GotStoresAction
>> = () => {
  return async (dispatch: Dispatch) => {
    const gettingStoresAction: GettingStoresAction = {
      type: 'GettingStores',
    };
    dispatch(gettingStoresAction);

    const stores = await getStores();

    const gotStoreAction: GotStoresAction = {
      stores,
      type: 'GotStores',
    };
    dispatch(gotStoreAction);
  };
};

export const storesReducer: Reducer<StoreState, StoreActions> = (
  state = initialStoreState,
  action,
) => {
  switch (action.type) {
    case 'GettingStores': {
      return {
        ...state,
        stores: null,
        loading: true,
      };
    }
    case 'GotStores': {
      return {
        ...state,
        stores: action.stores,
        loading: false,
      };
    }
    default:
      neverReached(action);
  }
  return state;
};

const neverReached = (never: never) => {};
