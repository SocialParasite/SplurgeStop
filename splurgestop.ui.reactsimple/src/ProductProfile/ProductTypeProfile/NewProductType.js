import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from '../../Components/Page';
import { addProductType } from './ProductTypeCommands';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.min.css';

export function NewProductType() {
  const [productType, setProductType] = useState(null);

  const handleInputChange = (event) => {
    setProductType({
      id: null,
      name: event.target.value,
    });
  };

  const notify = (info) => {
    toast.info(info);
  };

  const handleSubmit = async () => {
    let error = await addProductType({
      id: null,
      name: productType?.name,
    }).then(
      () => null,
      (productType) => productType,
    );

    if (error === null) {
      notify('Product type added');
    } else {
      toast.error(
        <div>
          Product type not added!
          <br />
          {error.message}
        </div>,
      );
    }
  };

  return (
    <Page title="Add a new product type">
      <Fragment>
        <ToastContainer />
        <div>
          <form onSubmit={handleSubmit}>
            <label>Product type name:</label>
            <input
              type="text"
              id="productTypeName"
              name="name"
              title="Product type name"
              onChange={handleInputChange}
              placeholder="product type name"
            />
            <input type="submit" value="Save" />
          </form>
        </div>
      </Fragment>
    </Page>
  );
}
