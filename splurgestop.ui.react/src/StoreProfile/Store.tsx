import React, { FC, Fragment } from 'react';
import { Link } from 'react-router-dom';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { StoreData } from './StoreData';

interface Props {
  data: StoreData;
}

export const Store: FC<Props> = ({ data }) => (
  <Fragment key={data.id}>
    <td>
      <Link
        css={css`
          text-decoration: none;
        `}
        to={`StoreInfo/${data.id}`}
      >
        {data.name}
      </Link>
    </td>
  </Fragment>
);
