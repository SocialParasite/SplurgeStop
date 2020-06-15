import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../Components/Page';
import { addPurchaseTransaction } from './PurchaseTransactionCommands';
import { useForm, Controller } from 'react-hook-form';
import id from 'uuid/v1';
import produce from 'immer';
import { set, has } from 'lodash';

function enhancedReducer(state, updateArg) {
  if (updateArg.constructor === Function) {
    return { ...state, ...updateArg(state) };
  }

  if (updateArg.constructor === Object) {
    if (has(updateArg, '_path') && has(updateArg, '_value')) {
      const { _path, _value } = updateArg;

      return produce(state, (draft) => {
        set(draft, _path, _value);
      });
    } else {
      return { ...state, ...updateArg };
    }
  }
}

const initialState = {
  transactionDate: '',
  storeId: '',
  notes: '',
  lineItems: [
    {
      id: id(),
      product: '',
      amount: '',
      currencyCode: 'EUR',
      currencySymbol: '€',
      booking: 'Credt',
      positionRelativeToPrice: 'end',
      notes: '',
    },
  ],
};

export function NewPurchaseTransaction() {
  const [state, updateState] = React.useReducer(enhancedReducer, initialState);
  const updateForm = React.useCallback(
    ({ target: { value, name, type } }) => {
      const updatePath = name.split('.');

      if (updatePath.length === 1) {
        const [key] = updatePath;

        updateState({
          [key]: value,
        });
      }

      if (updatePath.length === 2) {
        updateState({
          _path: updatePath,
          _value: value,
        });
      }
      console.log(state);
    },
    [state],
  );

  // const [transactionLineItems, setTransactionLineItems] = useState([]);
  // const { register, getValues, handleSubmit, control } = useForm();

  const [stores, setStores] = useState(null);
  const [storesLoading, setStoresLoading] = useState(true);

  // const totalPrice = () =>
  //   transaction.lineItems.reduce((sum, item) => sum + item.amount, 0);

  useEffect(() => {
    const loadStores = async () => {
      const url = 'https://localhost:44304/api/Store';
      const response = await fetch(url);
      const data = await response.json();
      setStores(data);
      setStoresLoading(false);
    };

    loadStores();
  }, []);

  const onSubmit = async () => {
    console.log(state.transactionDate);
    console.log(state.storeId);
    console.log(state.notes);
    console.log(state.lineItems);
    await addPurchaseTransaction({
      id: null,
      transactionDate: state.transactionDate,
      storeId: state.storeId,
      notes: state.notes,
      lineItems: [], //state.lineItems,
    });
  };

  // const onSubmit = (transactionLineItems) => {
  //   console.log(transactionLineItems);
  // };

  return (
    <Page title="Add new purchase transaction">
      <div>
        <Fragment>
          <div>
            {/* <form onSubmit={handleSubmit(onSubmit)}> */}
            <form onSubmit={onSubmit}>
              <div
                css={css`
                  margin: 1em;
                `}
              >
                <input
                  type="date"
                  className="input"
                  id="transactionDate"
                  name="transactionDate"
                  onChange={updateForm}
                  value={state.transactionDate}
                ></input>
              </div>
              {storesLoading ? (
                <div
                  css={css`
                    font-size: 16px;
                    font-style: italic;
                  `}
                >
                  Loading...
                </div>
              ) : (
                <div
                  css={css`
                    margin: 1em;
                  `}
                >
                  <label for="stores">Select a store:</label>
                  <select
                    name="storeId"
                    id="storeId"
                    className="input"
                    type="text"
                    onChange={updateForm}
                  >
                    {stores.map((store) => (
                      <option value={store.id}>{store.name}</option>
                    ))}
                  </select>
                </div>
              )}
              <div
                css={css`
                  margin: 1em;
                `}
              >
                TOTAL
                {/* Total: {totalPrice().toFixed(2)}{' '}
                {transaction.lineItems[0].currencySymbol}{' '} */}
              </div>
              <div
                css={css`
                  margin: 1em;
                  width: 50em;
                `}
              >
                <lable for="notes">Notes:</lable>
                <textarea
                  id="notes"
                  name="notes"
                  className="input"
                  type="text"
                  onChange={updateForm}
                  css={css`
                    width: 50em;
                  `}
                />
              </div>
              <input
                type="submit"
                className="btn btn-primary"
                value="Save"
                css={css`
                  margin: 1em 0 0 0;
                  width: 5.5em;
                `}
              />
            </form>
          </div>
        </Fragment>
      </div>
    </Page>
  );
}
