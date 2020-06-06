import React, { FC, useState, Fragment, useEffect } from 'react';
import { Page } from './../Page';
import { RouteComponentProps } from 'react-router-dom';
import { DetailedStoreData, getStore, postStore } from './StoreData';
import { Form, required, minLength, Values } from './../Form';
import { Field } from './../Field';

interface RouteParams {
  id: string;
}

export const StorePage: FC<RouteComponentProps<RouteParams>> = ({ match }) => {
  const [store, setStore] = useState<DetailedStoreData | null>(null);

  const [isEditing, setEditing] = useState(false);

  useEffect(() => {
    const doGetStore = async (id: string) => {
      const foundStore = await getStore(id);
      setStore(foundStore);
    };
    if (match.params.id) {
      const storeId = match.params.id;
      doGetStore(storeId);
    }
  }, [match.params.id]);

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const handleSubmit = async (values: Values) => {
    const modStore = await postStore({
      id: store?.id.value.toString(),
      name: values.name,
    });
    return { success: modStore ? true : false };
  };

  return (
    <Page title={store?.name}>
      <button onClick={editModeClick}>Edit</button>
      <div>
        {store !== null && (
          <Fragment>
            <div>
              {isEditing ? (
                <Form
                  submitCaption="Save"
                  onSubmit={handleSubmit}
                  validationRules={{
                    name: [
                      { validator: required },
                      { validator: minLength, arg: 1 },
                    ],
                  }}
                  failureMessage="There was a problem with your store"
                  successMessage="Your store update was successfully submitted"
                >
                  <Field name="name" label="Store name" />
                </Form>
              ) : (
                <div>
                  <h1>{store.name}</h1>
                </div>
              )}
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
};
