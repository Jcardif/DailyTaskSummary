param connections_todo_name string = 'todo'
param connections_keyvault_name string = 'keyvault'
param connections_keyvault_1_name string = 'keyvault-1'
param vaults_WorkSummaryVault_name string = 'WorkSummaryVault'
param workflows_DailyWorkSummary_name string = 'DailyWorkSummary'
param accounts_OAI_WorkSummary_name string = 'OAI-WorkSummary'

resource accounts_OAI_WorkSummary_name_resource 'Microsoft.CognitiveServices/accounts@2022-12-01' = {
  name: accounts_OAI_WorkSummary_name
  location: 'eastus'
  sku: {
    name: 'S0'
  }
  kind: 'OpenAI'
  properties: {
    customSubDomainName: 'oai-worksummary'
    publicNetworkAccess: 'Enabled'
  }
}

resource vaults_WorkSummaryVault_name_resource 'Microsoft.KeyVault/vaults@2022-11-01' = {
  name: vaults_WorkSummaryVault_name
  location: 'westeurope'
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: '72f988bf-86f1-41af-91ab-2d7cd011db47'
    accessPolicies: [
      {
        tenantId: '72f988bf-86f1-41af-91ab-2d7cd011db47'
        objectId: 'f836fd9d-8a86-4710-b121-e0093c31bce2'
        permissions: {
          keys: [
            'Get'
            'List'
            'Update'
            'Create'
            'Import'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
            'GetRotationPolicy'
            'SetRotationPolicy'
            'Rotate'
          ]
          secrets: [
            'Get'
            'List'
            'Set'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
          ]
          certificates: [
            'Get'
            'List'
            'Update'
            'Create'
            'Import'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
            'ManageContacts'
            'ManageIssuers'
            'GetIssuers'
            'ListIssuers'
            'SetIssuers'
            'DeleteIssuers'
          ]
        }
      }
      {
        tenantId: '72f988bf-86f1-41af-91ab-2d7cd011db47'
        objectId: '06c4b639-a108-4ebc-9f96-cd2810f18e70'
        permissions: {
          certificates: []
          keys: [
            'get'
            'list'
          ]
          secrets: [
            'get'
            'list'
          ]
        }
      }
    ]
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: false
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    enableRbacAuthorization: false
    vaultUri: 'https://worksummaryvault.vault.azure.net/'
    provisioningState: 'Succeeded'
    publicNetworkAccess: 'Enabled'
  }
}

resource connections_keyvault_name_resource 'Microsoft.Web/connections@2016-06-01' = {
  name: connections_keyvault_name
  location: 'eastus'
  kind: 'V1'
  properties: {
    displayName: 'WorkSummValutConn'
    statuses: [
      {
        status: 'Error'
        target: 'token'
        error: {
        }
      }
    ]
    customParameterValues: {
    }
    createdTime: '2023-03-12T20:29:10.733198Z'
    changedTime: '2023-03-12T20:29:10.733198Z'
    api: {
      name: connections_keyvault_name
      displayName: 'Azure Key Vault'
      description: 'Azure Key Vault is a service to securely store and access secrets.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1618/1.0.1618.3179/${connections_keyvault_name}/icon.png'
      brandColor: '#0079d6'
      id: '/subscriptions/a1a27566-3e3c-42d7-a372-692095cd8521/providers/Microsoft.Web/locations/eastus/managedApis/${connections_keyvault_name}'
      type: 'Microsoft.Web/locations/managedApis'
    }
    testLinks: []
  }
}

resource connections_keyvault_1_name_resource 'Microsoft.Web/connections@2016-06-01' = {
  name: connections_keyvault_1_name
  location: 'eastus'
  kind: 'V1'
  properties: {
    displayName: 'WorkSummVaultConn'
    statuses: [
      {
        status: 'Ready'
      }
    ]
    customParameterValues: {
    }
    createdTime: '2023-03-12T20:32:32.6852023Z'
    changedTime: '2023-03-12T20:32:32.6852023Z'
    api: {
      name: 'keyvault'
      displayName: 'Azure Key Vault'
      description: 'Azure Key Vault is a service to securely store and access secrets.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1618/1.0.1618.3179/keyvault/icon.png'
      brandColor: '#0079d6'
      id: '/subscriptions/a1a27566-3e3c-42d7-a372-692095cd8521/providers/Microsoft.Web/locations/eastus/managedApis/keyvault'
      type: 'Microsoft.Web/locations/managedApis'
    }
    testLinks: []
  }
}

resource connections_todo_name_resource 'Microsoft.Web/connections@2016-06-01' = {
  name: connections_todo_name
  location: 'eastus'
  kind: 'V1'
  properties: {
    displayName: 'jndemenge@microsoft.com'
    statuses: [
      {
        status: 'Connected'
      }
    ]
    customParameterValues: {
    }
    nonSecretParameterValues: {
    }
    createdTime: '2023-03-12T20:06:19.9975969Z'
    changedTime: '2023-03-13T14:48:49.8601713Z'
    api: {
      name: connections_todo_name
      displayName: 'Microsoft To-Do (Business)'
      description: 'Microsoft To-Do is an intelligent task management app that makes it easy to plan and manage your day. Connect to Microsoft To-Do to manage your tasks from various services. You can perform actions such as creating tasks.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1589/1.0.1589.2945/${connections_todo_name}/icon.png'
      brandColor: '#185ABD'
      id: '/subscriptions/a1a27566-3e3c-42d7-a372-692095cd8521/providers/Microsoft.Web/locations/eastus/managedApis/${connections_todo_name}'
      type: 'Microsoft.Web/locations/managedApis'
    }
    testLinks: [
      {
        requestUri: 'https://edge.management.azure.com:443/subscriptions/a1a27566-3e3c-42d7-a372-692095cd8521/resourceGroups/WorkSummary/providers/Microsoft.Web/connections/${connections_todo_name}/extensions/proxy/lists?api-version=2016-06-01'
        method: 'get'
      }
    ]
  }
}

resource accounts_OAI_WorkSummary_name_WorkSummDavinci 'Microsoft.CognitiveServices/accounts/deployments@2022-12-01' = {
  parent: accounts_OAI_WorkSummary_name_resource
  name: 'WorkSummDavinci'
  properties: {
    model: {
      format: 'OpenAI'
      name: 'code-davinci-002'
      version: '1'
    }
    scaleSettings: {
      scaleType: 'Standard'
    }
  }
}

resource accounts_OAI_WorkSummary_name_WorkSummGpt35 'Microsoft.CognitiveServices/accounts/deployments@2022-12-01' = {
  parent: accounts_OAI_WorkSummary_name_resource
  name: 'WorkSummGpt35'
  properties: {
    model: {
      format: 'OpenAI'
      name: 'gpt-35-turbo'
      version: '0301'
    }
    scaleSettings: {
      scaleType: 'Standard'
    }
  }
}

resource vaults_WorkSummaryVault_name_AOI_ApiKey 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: vaults_WorkSummaryVault_name_resource
  name: 'AOI-ApiKey'
  location: 'westeurope'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource vaults_WorkSummaryVault_name_AOI_EndPoint 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: vaults_WorkSummaryVault_name_resource
  name: 'AOI-EndPoint'
  location: 'westeurope'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource vaults_WorkSummaryVault_name_AOI_Location 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: vaults_WorkSummaryVault_name_resource
  name: 'AOI-Location'
  location: 'westeurope'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource workflows_DailyWorkSummary_name_resource 'Microsoft.Logic/workflows@2017-07-01' = {
  name: workflows_DailyWorkSummary_name
  location: 'eastus'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    state: 'Enabled'
    definition: {
      '$schema': 'https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#'
      contentVersion: '1.0.0.0'
      parameters: {
        '$connections': {
          defaultValue: {
          }
          type: 'Object'
        }
      }
      triggers: {
        Recurrence: {
          recurrence: {
            frequency: 'Hour'
            interval: 1
          }
          evaluatedRecurrence: {
            frequency: 'Hour'
            interval: 1
          }
          type: 'Recurrence'
        }
      }
      actions: {
        Filter_array: {
          runAfter: {
            Initialize_variable_4: [
              'Succeeded'
            ]
          }
          type: 'Query'
          inputs: {
            from: '@body(\'List_to-do\'\'s_by_folder_(V2)\')'
            where: '@equals(formatDateTime(item()?[\'lastModifiedDateTime\'], \'yyyy-MM-dd\'), formatDateTime(utcNow(), \'yyyy-MM-dd\'))'
          }
        }
        For_each_2: {
          foreach: '@body(\'Filter_array\')'
          actions: {
            Append_to_array_variable: {
              runAfter: {
                Select: [
                  'Succeeded'
                ]
              }
              type: 'AppendToArrayVariable'
              inputs: {
                name: 'TodaysItems'
                value: {
                  Description: '@{body(\'Parse_JSON_\')?[\'body\']?[\'content\']}'
                  Importance: '@{body(\'Parse_JSON_\')?[\'importance\']}'
                  Status: '@{body(\'Parse_JSON_\')?[\'status\']}'
                  SubTasks: '@body(\'Select\')'
                  TaskTitle: '@{body(\'Parse_JSON_\')?[\'title\']}'
                }
              }
            }
            Parse_JSON_: {
              runAfter: {
              }
              type: 'ParseJson'
              inputs: {
                content: '@items(\'For_each_2\')'
                schema: {
                  properties: {
                    '@@odata.context': {
                      type: 'string'
                    }
                    '@@odata.etag': {
                      type: 'string'
                    }
                    body: {
                      properties: {
                        content: {
                          type: 'string'
                        }
                        contentType: {
                          type: 'string'
                        }
                      }
                      type: 'object'
                    }
                    categories: {
                      type: 'array'
                    }
                    checklistItems: {
                      items: {
                        properties: {
                          createdDateTime: {
                            type: 'string'
                          }
                          displayName: {
                            type: 'string'
                          }
                          id: {
                            type: 'string'
                          }
                          isChecked: {
                            type: 'boolean'
                          }
                        }
                        required: [
                          'displayName'
                          'createdDateTime'
                          'isChecked'
                          'id'
                        ]
                        type: 'object'
                      }
                      type: 'array'
                    }
                    'checklistItems@odata.context': {
                      type: 'string'
                    }
                    createdDateTime: {
                      type: 'string'
                    }
                    hasAttachments: {
                      type: 'boolean'
                    }
                    id: {
                      type: 'string'
                    }
                    importance: {
                      type: 'string'
                    }
                    isReminderOn: {
                      type: 'boolean'
                    }
                    lastModifiedDateTime: {
                      type: 'string'
                    }
                    status: {
                      type: 'string'
                    }
                    title: {
                      type: 'string'
                    }
                  }
                  type: 'object'
                }
              }
            }
            Select: {
              runAfter: {
                Parse_JSON_: [
                  'Succeeded'
                ]
              }
              type: 'Select'
              inputs: {
                from: '@if(equals(body(\'Parse_JSON_\')?[\'checklistItems\'], null), json(\'[]\'), body(\'Parse_JSON_\')?[\'checklistItems\'])'
                select: {
                  displayName: '@item()?[\'displayName\']'
                  isChecked: '@item()?[\'isChecked\']'
                }
              }
            }
          }
          runAfter: {
            Filter_array: [
              'Succeeded'
            ]
          }
          type: 'Foreach'
        }
        Get_API_Key_Secret: {
          runAfter: {
            For_each_2: [
              'Succeeded'
            ]
          }
          type: 'ApiConnection'
          inputs: {
            host: {
              connection: {
                name: '@parameters(\'$connections\')[\'keyvault\'][\'connectionId\']'
              }
            }
            method: 'get'
            path: '/secrets/@{encodeURIComponent(\'AOI-ApiKey\')}/value'
          }
        }
        Initialize_Deployment_Name: {
          runAfter: {
            Get_API_Key_Secret: [
              'Succeeded'
            ]
          }
          type: 'InitializeVariable'
          inputs: {
            variables: [
              {
                name: 'DeploymentName'
                type: 'string'
                value: 'WorkSummDavinci'
              }
            ]
          }
        }
        Initialize_TaksInString: {
          runAfter: {
            Initialize_Deployment_Name: [
              'Succeeded'
            ]
          }
          type: 'InitializeVariable'
          inputs: {
            variables: [
              {
                name: 'TaksInString'
                type: 'string'
                value: '@{string(variables(\'TodaysItems\'))}'
              }
            ]
          }
        }
        Initialize_variable: {
          runAfter: {
            'List_to-do\'s_by_folder_(V2)': [
              'Succeeded'
            ]
          }
          type: 'InitializeVariable'
          inputs: {
            variables: [
              {
                name: 'Date'
                type: 'string'
                value: '@{formatDateTime(utcNow(), \'yyyy-MM-ddTHH:mm:ss.fffffffZ\')}'
              }
            ]
          }
        }
        Initialize_variable_2: {
          runAfter: {
            Initialize_variable: [
              'Succeeded'
            ]
          }
          type: 'InitializeVariable'
          inputs: {
            variables: [
              {
                name: 'TodaysItems'
                type: 'array'
              }
            ]
          }
        }
        Initialize_variable_3: {
          runAfter: {
            Initialize_TaksInString: [
              'Succeeded'
            ]
          }
          type: 'InitializeVariable'
          inputs: {
            variables: [
              {
                name: 'Prompt'
                type: 'string'
                value: 'Generate a summary of the work done based on the provided Json below. the summary should include all tasks with their accompanying subtasks and their status. Ensure the report is easy to understand and accurately reflects the work completed based on the JSON schema provided.\n\n@{variables(\'TaksInString\')}\n'
              }
            ]
          }
        }
        Initialize_variable_4: {
          runAfter: {
            Initialize_variable_2: [
              'Succeeded'
            ]
          }
          type: 'InitializeVariable'
          inputs: {
            variables: [
              {
                name: 'SubItems'
                type: 'array'
                value: []
              }
            ]
          }
        }
        'List_to-do\'s_by_folder_(V2)': {
          runAfter: {
          }
          type: 'ApiConnection'
          inputs: {
            host: {
              connection: {
                name: '@parameters(\'$connections\')[\'todo\'][\'connectionId\']'
              }
            }
            method: 'get'
            path: '/lists/@{encodeURIComponent(\'AAMkADI2ZTk0YjQ3LTY1ZWYtNDM1Ni04OWFkLTM0M2E4NDk4NDMzYQAuAAAAAACMX806WtwwQ6FFm5q4WRJNAQCIkBio4xPkQ6h5JVK7J1QWAAFu8I3XAAA=\')}/tasks'
            queries: {
              '$top': 999
            }
          }
        }
      }
      outputs: {
      }
    }
    parameters: {
      '$connections': {
        value: {
          keyvault: {
            connectionId: connections_keyvault_1_name_resource.id
            connectionName: 'keyvault-1'
            connectionProperties: {
              authentication: {
                type: 'ManagedServiceIdentity'
              }
            }
            id: '/subscriptions/a1a27566-3e3c-42d7-a372-692095cd8521/providers/Microsoft.Web/locations/eastus/managedApis/keyvault'
          }
          todo: {
            connectionId: connections_todo_name_resource.id
            connectionName: 'todo'
            id: '/subscriptions/a1a27566-3e3c-42d7-a372-692095cd8521/providers/Microsoft.Web/locations/westeurope/managedApis/todo'
          }
        }
      }
    }
  }
}