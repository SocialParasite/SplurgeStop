import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../../Components/Page';
import { updateProductType } from './ProductTypeCommands';

export function ProductTypePage({ match }) {
  const [productType, setProductType] = useState(null);
  const [productTypesLoading, setProductTypesLoading] = useState(true);
  const [isEditing, setEditing] = useState(false);

  useEffect(() => {
    const loadProductType = async () => {
      const id = match.params.id;
      const url = 'https://localhost:44304/api/ProductType/' + id;
      const response = await fetch(url);
      const data = await response.json();
      setProductType(data);
      setProductTypesLoading(false);
    };

    if (match.params.id) {
      const productTypeId = match.params.id;
      loadProductType(productTypeId);
    }
  }, [match.params.id]);

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const handleSubmit = async () => {
    await updateProductType({
      id: productType.id.value,
      name: productType.name,
    });
  };

  const changeHandler = (e) => {
    productType.name = e.currentTarget.value;
    setProductType(productType);
  };

  return (
    <Page title={productType?.name}>
      <Button onClick={editModeClick} className="float-right">
        Edit
      </Button>
      <div>
        {productTypesLoading ? (
          <div
            css={css`
              font-size: 16px;
              font-style: italic;
            `}
          >
            Loading...
          </div>
        ) : (
          <Fragment>
            <div
              css={css`
                margin-top: 5em;
              `}
            >
              {isEditing ? (
                <form onSubmit={handleSubmit}>
                  <input
                    type="text"
                    name="name"
                    label="Product type name"
                    placeholder={productType.name}
                    onChange={changeHandler}
                  />
                  <div
                    css={css`
                      margin: 1em;
                    `}
                  ></div>
                  <input type="submit" value="Save" />
                </form>
              ) : (
                <div>
                  <h1>{productType.name}</h1>
                </div>
              )}
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
}
