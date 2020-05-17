import React, { FC } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { PurchaseTransactionData } from './PurchaseTransactionData';

interface Props {
  data: PurchaseTransactionData;
}

export const PurchaseTransaction: FC<Props> = ({ data }) => (
  <tr
    css={css`
      text-align: right;
    `}
  >
    <td>{data.purchaseDate.toLocaleDateString()}</td>
    <td>{data.storeName}</td>
    <td>{data.totalPrice}</td>
    <td>{data.itemCount}</td>
  </tr>
);
