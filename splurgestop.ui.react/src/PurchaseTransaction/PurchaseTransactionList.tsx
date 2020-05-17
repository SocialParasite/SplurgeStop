import React, { FC } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { PurchaseTransactionData } from './PurchaseTransactionData';
import { PurchaseTransaction } from './PurchaseTransaction';

interface Props {
  data: PurchaseTransactionData[];
  renderItem?: (item: PurchaseTransactionData) => JSX.Element;
}

export const PurchaseTransactionList: FC<Props> = ({ data, renderItem }) => {
  return (
    <table>
      <tr
        css={css`
          text-align: left;
        `}
      >
        <th>Purchase date</th>
        <th>Store</th>
        <th>Total spent</th>
        <th>Item count</th>
      </tr>
      {data.map((transaction) => (
        <tbody key={transaction.purchaseTransactionId}>
          {renderItem ? (
            renderItem(transaction)
          ) : (
            <PurchaseTransaction data={transaction} />
          )}
        </tbody>
      ))}
    </table>
  );
};
